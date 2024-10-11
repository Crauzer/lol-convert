namespace lol_convert.Interfaces;

internal interface IMetaConvertable<T, TMeta>
{
    static abstract T FromMeta(TMeta metaClass);
}
