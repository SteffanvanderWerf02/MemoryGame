using MemoryGame.Models.Cards;
using MemoryGame.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace MemoryGame.Tools.CardDeserilizer
{
    public class MemoryCardListDeserilizer : JsonConverter
    {
        /// <summary>
        /// Check if Object can be Converted
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<IMemoryCard>);
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
            JArray array = JArray.Load(reader);
            List<IMemoryCard> result = new List<IMemoryCard>();

            foreach (JToken item in array)
            {
                if (item["imagePathFront"] != null)
                {
                    PokemonCard card = item.ToObject<PokemonCard>(serializer);
                    result.Add(card);
                }
                else if (item["cardValue"] != null)
                {
                    StandardCard card = item.ToObject<StandardCard>(serializer);
                    result.Add(card);
                }
                else
                {
                    throw new JsonSerializationException("Unable to determine the type of card.");
                }


            }

            return result;
        }

        /// <summary>
        /// Write object to Json
        /// </summary>
        /// <param name="writer">Writer for json</param>
        /// <param name="value">Value to be parsed</param>
        /// <param name="serializer">Serilizer to parse json</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JArray jo = JArray.FromObject(value, serializer);
            jo.WriteTo(writer);
        }
    }
}
