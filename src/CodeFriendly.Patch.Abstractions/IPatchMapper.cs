namespace CodeFriendly.Patch
{
    public interface IPatchMapper<TSource, TDestination> 
        where TSource : class 
        where TDestination : class
    {
        TSource CreateSourceObject(TDestination entity);
        TDestination CreateDestinationObject(TSource domain);
        TDestination PatchDestinationObject<TPatch>(TPatch source, TDestination target)
            where TPatch: TSource;
    }
}