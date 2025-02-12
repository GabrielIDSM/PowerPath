using System.ComponentModel;
using System.Reflection;

namespace PowerPath.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString()) ??
                throw new ArgumentException($"Nenhuma campo encontrado para o valor \"{value}\"", nameof(value));

            DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();

            if (attribute is not null)
            {
                return attribute.Description;
            }
            else
            {
                throw new ArgumentException($"Nenhuma descrição encontrada para o valor \"{value}\"", nameof(value));
            }
        }

        public static List<string> GetDescriptions<TEnum>() where TEnum : Enum
        {
            List<string> descriptions = [];
            Array values = Enum.GetValues(typeof(TEnum));

            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    FieldInfo? field = value.GetType().GetField(value.ToString()!);

                    if (field is not null)
                    {
                        DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();

                        if (attribute is not null)
                        {
                            descriptions.Add(attribute.Description);
                        }
                        else
                        {
                            descriptions.Add(value.ToString()!);
                        }
                    }
                }
            }

            return descriptions;
        }
    }
}
