// <auto-generated/>
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
namespace BookSample.ReviewApiClient.Models {
    public class Rating : IParsable 
    {
        /// <summary>The averageStars property</summary>
        public double? AverageStars { get; set; }
        /// <summary>The bookId property</summary>
        public long? BookId { get; set; }
        /// <summary>The numberOf1Star property</summary>
        public int? NumberOf1Star { get; set; }
        /// <summary>The numberOf2Stars property</summary>
        public int? NumberOf2Stars { get; set; }
        /// <summary>The numberOf3Stars property</summary>
        public int? NumberOf3Stars { get; set; }
        /// <summary>The numberOf4Stars property</summary>
        public int? NumberOf4Stars { get; set; }
        /// <summary>The numberOf5Stars property</summary>
        public int? NumberOf5Stars { get; set; }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="Rating"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static Rating CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new Rating();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                {"averageStars", n => { AverageStars = n.GetDoubleValue(); } },
                {"bookId", n => { BookId = n.GetLongValue(); } },
                {"numberOf1Star", n => { NumberOf1Star = n.GetIntValue(); } },
                {"numberOf2Stars", n => { NumberOf2Stars = n.GetIntValue(); } },
                {"numberOf3Stars", n => { NumberOf3Stars = n.GetIntValue(); } },
                {"numberOf4Stars", n => { NumberOf4Stars = n.GetIntValue(); } },
                {"numberOf5Stars", n => { NumberOf5Stars = n.GetIntValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteDoubleValue("averageStars", AverageStars);
            writer.WriteLongValue("bookId", BookId);
            writer.WriteIntValue("numberOf1Star", NumberOf1Star);
            writer.WriteIntValue("numberOf2Stars", NumberOf2Stars);
            writer.WriteIntValue("numberOf3Stars", NumberOf3Stars);
            writer.WriteIntValue("numberOf4Stars", NumberOf4Stars);
            writer.WriteIntValue("numberOf5Stars", NumberOf5Stars);
        }
    }
}
