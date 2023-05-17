using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

internal static class Utils
{
    #region "Manager Arrays"
    internal static string Append(this string array, string concat) => array + concat;
    internal static string Prepend(this string array, string concat) => concat + array;

    //equivalente a Prepend
    internal static T[] Push<T>(this T[] array, T item)
    {
        List<T> list = new(array);
        list.RemoveAt(array.Length - 1);
        array[0] = item;
        list.forEach((it, i) =>
        {
            array[i + 1] = it;
        });
        return array;
    }
    //equivalente a Append
    internal static T[] Put<T>(this T[] array, T item)
    {
        List<T> list = new(array);
        list.RemoveAt(0);
        list.Add(item);
        list.forEach((it, i) =>
        {
            array[i] = it;
        });
        return array;
    }

    internal static R[] Concat<T, R>(this T[] array, Func<T, R[]> action)
    {
        var lista = new List<R>();
        foreach (var it in array)
            lista.AddRange(action(it));
        return lista.ToArray();
    }
    internal static string Concat<T>(this List<T> array) => array.ToArray().Concat();
    internal static string Concat<T>(this T[] array)
    {
        var lista = "";
        foreach (var it in array)
            lista += it;
        return lista.Trim();
    }
    #region 'forEach'

    internal static IEnumerable<T> forEach<T>(this IEnumerable<T> array, Action<T> action) =>
    array.forEach((it, i) => action(it));

    internal static IEnumerable<T> forEach<T>(this IEnumerable<T> array, Func<T, T> func) =>
    array.forEach((it, i) => func(it));

    internal static IEnumerable<T> forEach<T>(this IEnumerable<T> array, Action<T, int> action) =>
    array.forEach((Func<T, int, T>)((it, i) => { action(it, i); return it; }));

    internal static IEnumerable<T> forEach<T>(this IEnumerable<T> array, Func<T, int, T> func)
    {
        var items = retorning(array, func);
        if (array.Count() != items.Count())
        {
            int index = 0;
            List<T> values = new List<T>();
            foreach (T it in array)
            {
                values.Add(func(it, index));
                index++;
            }
            return values;
        }
        return items;
    }
    static IEnumerable<T> retorning<T>(IEnumerable<T> array, Func<T, int, T> func)
    {
        if (array == null)
            throw new Exception("No debe ser null");
        int index = 0;
        foreach (var item in array)
        {
            T it = func.Invoke(item, index++);
            yield return it;
        }
    }

    #endregion

    #region 'filter'
    internal static IEnumerable<T> filter<T>(this IEnumerable<T> array, Predicate<T> func)
    {
        if (array == null) return null;

        List<T> Put = new();
        foreach (var it in array)
        {
            if (func(it)) Put.Add(it);
        }
        return Put;
    }
    #endregion

    #region 'Select'
    internal static IEnumerable<T> Select<T>(this IEnumerable<T> array, Func<T, T> func)
    {
        if (array == null) return null;

        List<T> Put = new();
        foreach (var it in array)
        {
            Put.Add(func(it));
        }
        return Put;
    }

    internal static ICollection<T> Select<T>(this ICollection<T> array, Func<T, T> func)
    {
        if (array == null) return null;

        List<T> Put = new();
        foreach (var it in array)
        {
            Put.Add(func(it));
        }
        return Put;
    }

    internal static IList<T> Select<T>(this IList<T> array, Func<T, T> func)
    {
        if (array == null) return null;

        List<T> Put = new();
        foreach (var it in array)
        {
            Put.Add(func(it));
        }
        return Put;
    }

    internal static T[] Select<T>(this T[] array, Func<T, T> func)
    {
        if (array == null) return null;

        List<T> Put = new();
        foreach (var it in array)
        {
            Put.Add(func(it));
        }
        return Put.ToArray();
    }
    #endregion

    #region 'Map'
    internal static IEnumerable<R> Map<T, R>(this IEnumerable<T> array, Func<T, R> func)
    {
        if (array == null) return null;

        List<R> Put = new();
        foreach (var it in array)
        {
            Put.Add(func(it));
        }
        return Put;
    }
    #endregion

    internal static E NextElement<E>(this IEnumerable<E> array, int index = 0) => index != (array.Count() - 1) ? array.ToArray()[++index] : default(E);
    internal static E PrevElement<E>(this IEnumerable<E> array, int index = 0) => index != 0 ? array.ToArray()[--index] : default(E);
    internal static bool hasNextElement<E>(this IEnumerable<E> array, int index = 0) => index != (array.Count() - 1);
    internal static bool hasPrevElement<E>(this IEnumerable<E> array, int index = 0) => index != 0;
    internal static string x(this string str, int count)
    {
        var result = str;
        for (int i = 0; i < count; i++)
        {
            result += str;
        }
        return result;
    }
    internal static string[] split(this string str, string wordSeparator)
    {
        var list = new List<string>();
        var i = 0;
        var variante = "";
        foreach (var it in str)
        {
            if (it == wordSeparator[i])
                ++i;
            else
                i = 0;
            variante += it;
            if (i == (wordSeparator.Length))
            {
                list.Add(variante.Replace(wordSeparator, ""));
                variante = "";
                i = 0;
            }
        }
        list.Add(variante);
        return list.ToArray();
    }
    internal static bool Has(this string str, params string[] text) => text.Map(txt => txt.ToLower() == str.ToLower()).Max();
    #endregion

    #region "Manager Streams"
    /// <summary>
    /// Read to End of a sequence.
    /// </summary>
    /// <param name="stream">object to extract sequence</param>
    /// <returns>An Array of bytes of sequece extracted</returns>
    internal static byte[] ReadToEnd(this Stream stream)
    {
        if (stream == null)
            return Array.Empty<byte>();

        int i = 200; byte[] bytes = new byte[200]; List<byte> lista = new();
        while ((i < 200 ? 0 : (i = stream.Read(bytes, 0, 200))) > 0)
            for (int index = 0; index != i; index++)
                lista.Add(bytes[index]);
        return lista.ToArray();
    }
    /// <summary>
    /// Read to End of a sequence async way.
    /// </summary>
    /// <param name="stream">object to extract sequence</param>
    /// <returns>An Array of bytes of sequece extracted</returns>
    internal static async System.Threading.Tasks.Task<byte[]> ReadToEndAsync(this Stream stream)
    {
        if (stream == null)
            return Array.Empty<byte>();

        int i = 200; byte[] bytes = new byte[200]; List<byte> lista = new();
        while ((i < 200 ? 0 : (i = await stream.ReadAsync(bytes, 0, 200))) > 0)
            for (int index = 0; index != i; index++)
                lista.Add(bytes[index]);
        return lista.ToArray();
    }

    internal static T Cast<T>(this byte[] array, Func<byte[], T> func) => func(array);

    #endregion

    #region "booleans"
    /// <summary>
    /// Tras la llamada de un booleano si la respuesta es verdadero ejecuta el delagado.
    /// No modifica el valor de salida.
    /// </summary>
    /// <param name="conditional">condicionar</param>
    /// <param name="action">delegado a ejecutar al ser verdadero el valor</param>
    /// <returns>devuelve la respuesta sea verdadero o falso</returns>
    internal static bool Callback(this Boolean conditional, Action action)
    {
        if (conditional)
            action();
        return conditional;
    }
    /// <summary>
    /// Tras la llamada de un booleano si la respuesta es falsa ejecuta el delagado.
    /// No modifica el valor de salida.
    /// </summary>
    /// <param name="conditional">condicionar</param>
    /// <param name="action">delegado a ejecutar al ser falso el valor</param>
    /// <returns>devuelve la respuesta sea verdadero o falso</returns>
    internal static bool Else(this Boolean conditional, Action action)
    {
        if (!conditional)
            action();
        return conditional;
    }
    #endregion

    #region "Manager Files"
    internal static IEnumerable<T> findPaths<T>(this IEnumerable<T> array, Func<T, IEnumerable<T>> action)
    {
        List<T> paths = array.ToList();
        for (int i = 0; i < paths.Count; i++)
            paths.AddRange(action(paths[i]));
        return paths;
    }
    internal static T[] findPaths<T>(this T[] array, Func<T, T[]> action)
    {
        List<T> paths = array.ToList();
        for (int i = 0; i < paths.Count; i++)
            paths.AddRange(action(paths[i]));
        return paths.ToArray();
    }
    #endregion

    #region "Objects"
    internal static T After<T>(this T obj, Action action)
    {
        action();
        return obj;
    }
    #endregion

    /// <summary>
    /// Valida los patrones que se encuentra en la cadena actual.
    /// '#'-> <see cref="int"/> Is a wherever number.
    /// '%'-> <see cref="String"/> This is lower letter.
    /// '^'-> <see cref="String"/> Upper letter.
    /// '*'-> <see cref="Char"/> wherever char that not is letter or digit.
    /// More than one is explicit.
    /// </summary>
    /// <param name="array"> the Chars Array</param>
    /// <param name="pattern">pattern by validate</param>
    /// <returns>Devuelve true si el patron coincide con la cadena.</returns>
    internal static bool patterns(this string array, string pattern)
    {
        bool isPattern(char p) => p == '#' || p == '$' || p == '%' || p == '^' || p == '*';
        bool isSymbol(char it) => !isPattern(it) && !Char.IsLetterOrDigit(it);
        List<string> patterns = new();
        List<string> keys = new();
        var variante = "";
        int state = 0; //0 is lower, 1 is upper, 2 is digit, 3 is pattern
        void AddItemP() { patterns.Add(variante); variante = ""; };
        void AddItemK() { keys.Add(variante); variante = ""; };
        void sentIdea(int st, char it, int i)
        {
            if (state != st && !String.IsNullOrEmpty(variante))
                AddItemP();
            variante += it;
            if ((pattern.Length - 1) == i)
                AddItemP();
            state = st;
        };
        bool patternValid(char p, char d) => (p) switch
        {
            '#' => Char.IsDigit(d),
            '$' => Char.IsLetter(d),
            '%' => Char.IsLower(d),
            '^' => Char.IsUpper(d),
            '*' => true,
            _ => p == d
        };
        foreach ((var it, var i) in new (char, int)[pattern.Length].forEach((c, i) => (pattern[i], i)))
        {
            if (Char.IsLower(it) || isSymbol(it))
            {
                sentIdea(0, it, i);
                continue;
            }
            if (Char.IsUpper(it) || isSymbol(it))
            {
                sentIdea(1, it, i);
                continue;
            }
            if (Char.IsDigit(it))
            {
                sentIdea(2, it, i);
                continue;
            }
            if (isPattern(it))
            {
                sentIdea(3, it, i);
                if (pattern.hasNextElement(i))
                    if (pattern.NextElement(i) != it)
                        AddItemP();
                continue;
            }

        }
        variante = "";
        int secuencia = 0;
        foreach ((var it, var i) in new (string, int)[patterns.Count].forEach((c, i) => (patterns[i], i)))
        {
            if (isPattern(it.First())) //es patron
                if (it.Length > 1)//patron contiene cantidades explicitas de caracteres
                {
                    variante = array.Substring(secuencia, it.Length);
                    secuencia += it.Length;
                    AddItemK();
                    continue;
                }
                else //si no entonces obtiene todos los carecteres dinamicamente
                {
                    if (it == "*" && (i != patterns.Count - 1))//si el patron es '*' y no es el ultimo elemento en la cola
                    {
                        var sptt = patterns.NextElement(i);
                        if (!isPattern(sptt[0]))//sptt patron explicito
                        {
                            var restante = array.Substring(secuencia);
                            variante = restante.split(sptt)[0];
                            if (variante.Length == 0)
                                variante = $"{sptt}{restante.Split(sptt.ToArray()).NextElement()}";
                            secuencia += variante.Length;
                        }
                        else//si es patron que se detenga hasta el siguiente patron
                            for (_ = secuencia; (secuencia != array.Length) ? patternValid(it[0], array[secuencia]) && !patternValid(sptt[0], array[secuencia]) : false; secuencia++)
                                variante += array[secuencia];

                        AddItemK();
                        continue;
                    }
                    for (_ = secuencia; secuencia != array.Length ? patternValid(it[0], array[secuencia]) : false; secuencia++)
                        variante += array[secuencia];
                    AddItemK();
                }
            else // es patron explicito
            {
                variante = array.Substring(secuencia, it.Length);
                secuencia += it.Length;
                AddItemK();
                continue;
            }
        }
        string rebuild = keys.Concat();
        return array.Equals(rebuild);
    }

}
namespace System.Runtime.CompilerServices
{
    internal class IsExternalInit
    {

    }
}

