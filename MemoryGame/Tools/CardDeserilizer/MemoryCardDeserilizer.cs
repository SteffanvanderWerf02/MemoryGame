using MemoryGame.Models.Cards;
using MemoryGame.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace MemoryGame.Tools.CardDeserilizer
{
    public class MemoryCardDeserilizer : JsonConverter
    {
        /// <summary>
        /// Check if Object can be Converted
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IMemoryCard);
        }

        /// <summary>
        /// Read json and parse to object
        /// </summary>
        /// <param name="reader">Json reader to read the json</param>
        /// <param name="objectType">Object type of the json</param>
        /// <param name="existingValue">Existing value of the type</param>
        /// <param name="serializer">Way to serilize the json</param>
        /// <returns>Returns object of a pokemon card or standard card</returns>
        /// <exception cref="JsonSerializationException">Throws when type is not found</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                JObject obj = JObject.Load(reader);
                IMemoryCard card = null;

                // Deserialize the drawn card based on its properties
                if (obj["imagePathFront"] != null)
                {
                    card = obj.ToObject<PokemonCard>(serializer);
                }
                else if (obj["cardValue"] != null)
                {
                    card = obj.ToObject<StandardCard>(serializer);
                }
                else
                {
                    throw new JsonSerializationException("Unable to determine the type of card.");
                }

                return card;
            }
            catch (Exception ex) {
                return null;
            }
            
            
        }


          

        /// <summary>
        /// Write object to Json
        /// </summary>
        /// <param name="writer">Writer for json</param>
        /// <param name="value">Value to be parsed</param>
        /// <param name="serializer">Serilizer to parse json</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jo = JObject.FromObject(value, serializer);
            jo.WriteTo(writer);
        }
    }
}
