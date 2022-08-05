using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

using System.Drawing;

using LostArkLogger.LarkCustom.Units;
using LostArkLogger.LarkCustom.Utility;

namespace LostArkLogger.LarkCustom.Config
{
    public class ClassBrushes
    {
        private Dictionary<string, Brush[]> brushesByString = new Dictionary<string, Brush[]>();
        private Dictionary<PlayerClassEnum, Brush[]> brushesByEnum = new Dictionary<PlayerClassEnum, Brush[]>();

        public ClassBrushes()
        {
            foreach (PlayerClassEnum playerClass in Enum.GetValues(typeof(PlayerClassEnum)))
            {
                brushesByString[playerClass.ToString()] = new Brush[2];
                brushesByEnum[playerClass] = new Brush[2];
            }
        }
        public ClassBrushes(bool autoFill)
        {
            foreach (PlayerClassEnum playerClass in Enum.GetValues(typeof(PlayerClassEnum)))
            {
                brushesByString[playerClass.ToString()] = new Brush[2];
                brushesByEnum[playerClass] = new Brush[2];
            }
            Dictionary<string, int[]> bepis;
        }

        public Brush GetClassBackgroundColor(PlayerClassEnum playerClass)
        {
            return brushesByEnum[playerClass][0];
        }
        public Brush GetClassBackgroundColor(string playerClass)
        {
            if (brushesByString.ContainsKey(playerClass))
            {
                return brushesByString[playerClass][0];
            }
            return null;
        }

        public Brush GetClassTextColor(PlayerClassEnum playerClass)
        {
            return brushesByEnum[playerClass][1];
        }

        public Brush GetClassTextColor(string playerClass)
        {
            if (brushesByString.ContainsKey(playerClass))
            {
                return brushesByString[playerClass][1];
            }
            return null;
        }

        public void SetClassBackgroundColor(PlayerClassEnum playerClass, Color color)
        {
            Brush b = new SolidBrush(color);
            brushesByEnum[playerClass][0] = b;
            brushesByString[playerClass.ToString()][0] = b;
        }

        public void SetClassTextColor(PlayerClassEnum playerClass, Color color)
        {
            Brush b = new SolidBrush(color);
            brushesByEnum[playerClass][1] = b;
            brushesByString[playerClass.ToString()][1] = b;
        }
    }


    public class ClassBrushesConverter : JsonConverter<ClassBrushes>
    {

        public override ClassBrushes? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            ClassBrushes cb = new ClassBrushes();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return cb;
                }

                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string? propertyName = reader.GetString();

                // For performance, parse with ignoreCase:false first.
                if (!Enum.TryParse(propertyName, ignoreCase: false, out PlayerClassEnum key) &&
                    !Enum.TryParse(propertyName, ignoreCase: true, out key))
                {
                    throw new JsonException(
                        $"Unable to convert \"{propertyName}\" to Enum \"PlayerClassEnum\".");
                }

                // Get the value.
                int[] colors = JsonSerializer.Deserialize<int[]>(ref reader, options)!;

                // Add to dictionary.
                Color backgroundColor = Color.FromArgb(colors[0]);
                Color textColor = Color.FromArgb(colors[1]);

                cb.SetClassTextColor(key, textColor);
                cb.SetClassBackgroundColor(key, backgroundColor);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ClassBrushes classBrush, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (PlayerClassEnum playerClass in Enum.GetValues(typeof(PlayerClassEnum)))
            {

                Brush backgroundColor = classBrush.GetClassBackgroundColor(playerClass);
                Brush textColor = classBrush.GetClassTextColor(playerClass);

                int bgColInt = ((SolidBrush)backgroundColor).Color.ToArgb();
                int textColInt = ((SolidBrush)textColor).Color.ToArgb();

                var propertyName = playerClass.ToString();
                writer.WritePropertyName
                    (options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName);


                JsonSerializer.Serialize<int[]>(writer, new int[] { bgColInt, textColInt }, options);
            }

            writer.WriteEndObject();
        }
    }
}


