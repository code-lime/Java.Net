using System.Collections.Generic;
using Java.Net.Model.Raw.Annotation;

namespace Java.Net.Model.Raw.Base;

public interface IAnnotations
{
    List<IAnnotation> Annotations { get; set; }
}
