namespace Chameleon.Utils
{
    public class PropertyCopier<TSource, TDestination> 
    {
        public static void Copy(TSource source, TDestination destination)
        {
            var parentProperties = source.GetType().GetProperties();
            var childProperties = destination.GetType().GetProperties();

            foreach (var parentProperty in parentProperties)
            {
                foreach (var childProperty in childProperties)
                {
                    if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                    {
                        childProperty.SetValue(destination, parentProperty.GetValue(source));
                        break;
                    }
                }
            }
        }
    }
}