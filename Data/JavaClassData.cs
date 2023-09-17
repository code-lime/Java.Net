using Java.Net.Data.Signature;
using Java.Net.Flags;
using Java.Net.Model;
using Java.Net.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Java.Net.Data
{
    public abstract class IJavaClassData
    {
        public const uint MagicNumClass = 3405691582;

        public uint MagicNum { get; set; }
        public Version JavaVersion { get; set; }

        public AccessClass Flags { get; set; }
        public string Name { get; set; }

        public SuperclassSignature SuperClass { get; set; }
        public List<FormalTypeParameter> FormalTypeParameters { get; set; } = new List<FormalTypeParameter>(); 
        public List<SuperinterfaceSignature> Interfaces { get; set; } = new List<SuperinterfaceSignature>();
        public List<ChildJavaClassData> Childs { get; set; } = new List<ChildJavaClassData>();
        public List<JavaAnnotationData> Annotations { get; set; } = new List<JavaAnnotationData>();
        public List<JavaFieldData> Fields { get; set; } = new List<JavaFieldData>();

        protected List<(JavaClass child, IJavaClassData parent, IJavaClassData instance)> ToJava(IJavaClassData parent, string old_full_name, TypeVariableReader variable)
        {
            string full_name = this.GetFullName(old_full_name);
            variable = variable.Add(FormalTypeParameters);
            List<(JavaClass child, IJavaClassData parent, IJavaClassData instance)> classes = new List<(JavaClass child, IJavaClassData parent, IJavaClassData instance)>();
            foreach (var child in Childs) classes.AddRange(child.ToJava(this, full_name, variable));
            classes.Add((JavaClass.Create(full_name, SuperClass.ClassTypeSignature.SimpleClassTypeSignature.SignatureFormat, (dat) =>
            {
                dat.MagicNum = MagicNum;
                dat.JavaVersion = JavaVersion;
                dat.Flags = Flags;
                ClassSignature signature = new ClassSignature()
                {
                    SuperclassSignature = SuperClass,
                    FormalTypeParameters = FormalTypeParameters.Count == 0 ? null : new FormalTypeParameters() { FormalTypeParameter = FormalTypeParameters.ToArray() }
                };
                foreach (var _interface in Interfaces)
                {
                    signature.SuperinterfaceSignature.Add(_interface);
                    dat.Interfaces.Add(dat.OfConstant(ClassConstant.Create(dat, v => v.Name = _interface.ClassTypeSignature.SimpleClassTypeSignature.SignatureFormat)));
                }
                dat.Attributes.Add(SignatureAttribute.Create(dat, v =>
                {
                    v.Signature = Utf8Constant.Create(dat, _v => _v.Value = signature.SignatureFormat);
                }));
                dat.Attributes.Add(InnerClassesAttribute.Create(dat, v =>
                {
                    v.Classes = classes.Select(clazz => InnerClassesAttribute.ClassInfo.Create(dat, _v =>
                    {
                        _v.InnerClassAccessFlags = dat.Flags;
                        _v.InnerName = Utf8Constant.Create(dat, __v => __v.Value = clazz.instance.Name);
                        string inner_class = clazz.child.ThisClass.Name;
                        _v.InnerClassInfo = ClassConstant.Create(dat, __v => __v.Name = inner_class);
                        _v.OuterClassInfo = ClassConstant.Create(dat, __v => __v.Name = string.Join("$", inner_class.Split('$')[..^1]));
                    })).ToList();
                }));
                if (Annotations.Count > 0) dat.Attributes.Add(RuntimeInvisibleAnnotationsAttribute.Create(dat, v =>
                {
                    v.Annotations = Annotations.Select(_v => _v.ToJava(dat)).ToList();
                }));
                foreach (var _field in Fields) dat.Fields.Add(_field.ToJava(dat, variable));
            }), parent, this));
            return classes;
        }
        protected abstract string GetFullName(string old_full_name);
    }
    public sealed class JavaClassData : IJavaClassData
    {
        public string[] Package { get; set; }
        public string FullName
        {
            get => $"{string.Join('/', Package)}{(Package.Length == 0 ? "" : "/")}{Name}";
            set
            {
                string[] args = value.Split('/');
                if (args.Length == 1)
                {
                    Package = new string[0];
                    Name = args[0];
                    return;
                }
                Package = args[..^1];
                Name = args[^1];
            }
        }

        public List<JavaClass> ToJava() => ToJava(null, null, TypeVariableReader.EMPTY).Select(v => v.child).ToList();

        protected override string GetFullName(string old_full_name) => FullName;
    }
    public sealed class ChildJavaClassData : IJavaClassData
    {
        protected override string GetFullName(string old_full_name) => (old_full_name == null ? "" : $"{old_full_name}$") + this.Name;
    }
}
