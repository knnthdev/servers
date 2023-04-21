using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antecesor
{
    internal class VisualUtils
    {
        /// <summary>
        /// Solo para el margen de 'Bottom' y 'Rigth'
        /// </summary>
        /// <param name="sizeparent">
        /// Atura del contenedor.
        /// </param>
        /// <param name="sizechild">Altura del objetivo</param>
        /// <param name="margin">Margin establecido a convertir</param>
        /// <returns>Devuelve un entero que representa un punto ubicado en pantalla.</returns>
        internal static int MarginBottomRigth(int sizeparent, int sizechild, int margin) => (sizeparent - sizechild) - margin;

    }
}
