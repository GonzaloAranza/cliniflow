using System.Security.Cryptography;
using System.Text;

namespace CliniFlow.Application.Utils;

public static class CodeGenerator
{
    // Usamos caracteres que no se confundan (sin I, 1, O, 0)
    private static readonly char[] Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

    public static string GenerateAppointmentCode(int length = 6)
    {
        var data = new byte[length];

        // Usamos criptografía para mayor aleatoriedad que 'Random'
        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }

        var result = new StringBuilder(length);
        foreach (var b in data)
        {
            result.Append(Chars[b % Chars.Length]);
        }

        return result.ToString(); // Ej: "K9P2XN"
    }
}