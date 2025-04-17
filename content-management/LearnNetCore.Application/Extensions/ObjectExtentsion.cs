using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LearnNetCore.Application.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Gán giá trị cho thuộc tính của đối tượng theo tên thuộc tính.
    /// </summary>
    public static T SetPropertyValue<T>(this T obj, string propertyName, object propertyValue)
    {
        if (obj == null || string.IsNullOrWhiteSpace(propertyName))
            return obj;

        PropertyInfo property = obj.GetType().GetProperty(propertyName);
        if (property == null || !property.CanWrite)
            return obj;

        Type propertyType = property.PropertyType;

        // Nếu là kiểu nullable và giá trị truyền vào là null hoặc rỗng
        if (IsNullable(propertyType) && (propertyValue == null || string.IsNullOrWhiteSpace(propertyValue.ToString())))
        {
            property.SetValue(obj, null);
            return obj;
        }

        try
        {
            object convertedValue = ConvertToPropertyType(propertyType, propertyValue);
            property.SetValue(obj, convertedValue);
        }
        catch
        {
            // Có thể log lỗi nếu cần
        }

        return obj;
    }

    /// <summary>
    /// Lấy giá trị thuộc tính theo tên thuộc tính.
    /// </summary>
    public static object GetPropertyValue(this object obj, string propertyName)
    {
        return obj?.GetType().GetProperty(propertyName)?.GetValue(obj);
    }

    /// <summary>
    /// Kiểm tra xem đối tượng có chứa thuộc tính theo tên hay không.
    /// </summary>
    public static bool HasProperty(this object obj, string propertyName)
    {
        return obj?.GetType().GetProperty(propertyName) != null;
    }

    /// <summary>
    /// Kiểm tra xem kiểu dữ liệu có chứa thuộc tính theo tên hay không.
    /// </summary>
    public static bool HasProperty(this Type type, string propertyName)
    {
        return type?.GetProperty(propertyName) != null;
    }

    /// <summary>
    /// Tạo bản sao đối tượng bằng cách serialize và deserialize.
    /// </summary>
    public static T DeepClone<T>(this T source)
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source));
    }

    /// <summary>
    /// Kiểm tra kiểu có phải là Nullable hay không.
    /// </summary>
    private static bool IsNullable(Type type)
    {
        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    /// Chuyển giá trị sang kiểu của thuộc tính.
    /// </summary>
    private static object ConvertToPropertyType(Type targetType, object value)
    {
        if (value == null)
            return null;

        Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
        return Convert.ChangeType(value, underlyingType);
    }
}
