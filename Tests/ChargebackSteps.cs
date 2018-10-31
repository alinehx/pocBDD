using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using Tests.Helper;
using Xunit;
using Xunit.Gherkin.Quick;

namespace Tests
{
   
    [FeatureFile("./Chargeback.Feature")]

    public sealed class ChargebackSteps: Feature
    {
        private dynamic OrderJson;
        private dynamic NewMovementJson;
        private readonly string url = "http://localhost:3500";
        
         [Given(@"I've a existing  movement order")]
        public void GivenIhaveaExistingMovementOrder()
        {
            object bodyOrderMovement = new
            {
                id = 1,
                movements = new[] {
                    new {type = "Normal", value = 20, chargeBack = false},
                    new { type = "Payment", value = 100, chargeBack = false}
                }
            };
            string urlCreateOrderMovement = string.Format($"{url}/api/orders/movement");
            IRestResponse responseApi = Api.Post(urlCreateOrderMovement, bodyOrderMovement);
            var statusCode = responseApi.StatusCode;
            statusCode.Equals(HttpStatusCode.Created);

            OrderJson = JObject.Parse(responseApi.Content);
        }



        [When(@"a generate a new movement")]
        public void WhenaGenerateaNewMovement()
        {
            object bodyNewMovement = new
            {
                id = 1,
                movements = new[] {
                    new {type = "Refused", value = 25, chargeBack = false},
                    new { type = "Payment", value = 100, chargeBack = false}
                }
            };

            string urlCreateOrderMovement = string.Format($"{url}/api/orders/movement/1");
            IRestResponse responseApi = Api.Post(urlCreateOrderMovement, bodyNewMovement);
            var statusCode = responseApi.StatusCode;
            statusCode.Equals(HttpStatusCode.Created);

            NewMovementJson = JObject.Parse(responseApi.Content);
        }

        [And(@"generate movement doesn't have existing movement")]
        public void WhenGenerateMovementDoesntHaveExistingMovement()
        {
            bool jsonOrderAndNewMovementAreEqual = Compare.JsonAreEqual(OrderJson, NewMovementJson);
            Assert.False(jsonOrderAndNewMovementAreEqual);
        }

        [And(@"existing movement is not a chargeback")]
        public void WhenExistingMovementIsNotAChargeback()
        {
            var verifyOrderExistingChargeBack = Compare.ExistingBooleanTrue(OrderJson.@movements);
            Assert.False(verifyOrderExistingChargeBack);
        }


        [Then(@"change this existing movement for Chargeback")]
        public void ThenChangeThisExistingMovementForChargeback()
        {
            var movementChargeBack = Compare.ReturnObjectIfBooleanIsTrue(NewMovementJson.@movements);
       
            if (movementChargeBack.Item1)
            {
                var objectExistingInOldMovements = Compare.VerifyObjectExistingInJArray(movementChargeBack.Item2, OrderJson.@movements);
                Assert.True(objectExistingInOldMovements);
            }            
        }

      

    }
}
