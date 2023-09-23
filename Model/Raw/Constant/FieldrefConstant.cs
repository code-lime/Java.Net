namespace Java.Net.Model.Raw.Constant;

public sealed class FieldrefConstant : IRefConstant<FieldrefConstant>
{
    public override ConstantTag Tag => ConstantTag.Fieldref;
}
