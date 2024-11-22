using System;

public static class AvatarHelper 
{
    private static readonly string[] Colors = new[] {
        "#1abc9c", "#2ecc71", "#3498db", "#9b59b6", "#34495e",
        "#16a085", "#27ae60", "#2980b9", "#8e44ad", "#2c3e50",
        "#f1c40f", "#e67e22", "#e74c3c", "#95a5a6", "#f39c12"
    };

    public static string GetInitialAvatar(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            return string.Empty;

        var initial = (!string.IsNullOrEmpty(firstName) ? firstName[0].ToString() : "") +
                     (!string.IsNullOrEmpty(lastName) ? lastName[0].ToString() : "");

        var random = new Random(initial.GetHashCode());
        var backgroundColor = Colors[random.Next(Colors.Length)];

        var svg = $@"<svg xmlns='http://www.w3.org/2000/svg' width='40' height='40'>
                       <rect width='40' height='40' fill='{backgroundColor}'/>
                       <text x='50%' y='50%' font-size='20' fill='white' text-anchor='middle' dy='.3em'>{initial}</text>
                     </svg>";

        // Convert the SVG to a base64 string
        var bytes = System.Text.Encoding.UTF8.GetBytes(svg);
        var base64 = Convert.ToBase64String(bytes);

        return $"data:image/svg+xml;base64,{base64}";
    }
}