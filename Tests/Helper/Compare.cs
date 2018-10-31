using Newtonsoft.Json.Linq;

namespace Tests.Helper
{
    public static class Compare
    {
        public static bool JsonAreEqual(dynamic source, dynamic target)
        {
           return JToken.DeepEquals(source, target);
        }


        public static (bool, dynamic) ReturnObjectIfBooleanIsTrue(JArray source)
        {
            bool existing = false;
            dynamic objectItem = null;
            foreach (var item in source)
            {
                if (item["chargeBack"].ToObject<bool>())
                {
                    existing = true;
                    objectItem = item;
                    break;
                }
            }
            return (existing, objectItem);
        }

        public static bool ExistingBooleanTrue(JArray source)
        {
            bool existingValueEqualTrue = false;           
            foreach (var item in source)
            {
               if (item["chargeBack"].ToObject<bool>())
               {
                    existingValueEqualTrue = true;                    
                    break;
               }
            }
            return existingValueEqualTrue;
        }

        public static bool VerifyObjectExistingInJArray(dynamic item, JArray source)
        {
            var type = item["type"].ToObject<string>();
           
            bool existing = false;
            foreach (var objectSource in source)
            {
                if (objectSource["type"].ToObject<string>() == type)
                {
                    existing = true;
                    break;
                }
            }
            return existing;
        }
        
    }
}
