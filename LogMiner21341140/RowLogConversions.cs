using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevComponents.DotNetBar;

namespace LogMiner
{
    class RowLogConversions
    {
        public List<string> ParseRowLogContents(string rowlogcontentzero, List<MetaData> metadata, string op, int pos )
        {
            var result = new List<string>();


            if (op == "LOP_MODIFY_ROW")
            {
                int cont;
                int posicion = 0;
                if (pos == -1)
                {
                 
                    for (int i = 0, n = 0; i < metadata.Count; i++)
                    {
                        result.Add(Conversions.Converter(rowlogcontentzero.Substring(n, metadata.ElementAt(i).Len * 2),
                            metadata.ElementAt(i).Type));
                        n += metadata.ElementAt(i).Len * 2;
                    }
                }
                else
                {
                    for (int i = 0, n = 0; i < metadata.Count - pos; i++)
                    {
                        result.Add(Conversions.Converter(rowlogcontentzero.Substring(n, metadata.ElementAt(i).Len*2),
                            metadata.ElementAt(i).Type));
                        n += metadata.ElementAt(i).Len*2;
                        posicion = n;
                    }
                    var vlengthColumns = rowlogcontentzero.Substring(posicion + 10);

                    //get delimiters from varchar
                    cont = 0;
                    var delimiters = new List<int> {(pos*4)};
                    for (var i = 0; i < pos; i++)
                    {
                        delimiters.Add(((Conversions.ToSmallInt(vlengthColumns.Substring(cont, 4))*2)) -
                                       (posicion + 12));
                        cont = cont + 4;
                    }

                    //get varlength values
                    for (var i = 0; i < pos; i++)
                    {
                        if (i != pos - 1)
                        {

                            var toAdd =
                                Conversions.Converter(
                                    vlengthColumns.Substring(delimiters[i], delimiters[i + 1] - (delimiters[i])),
                                    metadata.ElementAt(metadata.Count - pos + i).Type);
                            result.Add(toAdd);
                        }
                        else
                        {
                            var toAdd = Conversions.Converter(vlengthColumns.Substring(delimiters[i]),
                                metadata.ElementAt(metadata.Count - 1).Type);
                            result.Add(toAdd);
                        }
                    }
                }
            }
            else 
            //starts with 3 = varlenght fields exists in hex
            if (rowlogcontentzero.StartsWith("3"))
            {
                //where is the variable length offset is located
                var vLengthOffset = (Conversions.ToSmallInt(rowlogcontentzero.Substring(4, 4))*2) - 2;

                //remove the first 4 bytes
                rowlogcontentzero = rowlogcontentzero.Substring(8);

                //get the number of varlength columns
                var vlengthn = Conversions.ToSmallInt(rowlogcontentzero.Substring(vLengthOffset, 4));

                //get data from varlengthcolumns
                var vlengthColumns = rowlogcontentzero.Substring(vLengthOffset + 4);

                //convert fixed length values
                var cont = 0;
                for (var i = 0; i < metadata.Count - vlengthn; i++)
                {
                    result.Add(Conversions.Converter(rowlogcontentzero.Substring(cont, metadata[i].Len*2),
                        metadata[i].Type));
                    cont = cont + metadata.ElementAt(i).Len*2;
                }

                //get delimiters from varchar
                cont = 0;
                var delimiters = new List<int> {(vlengthn*4)};
                for (var i = 0; i < vlengthn; i++)
                {
                    delimiters.Add(((Conversions.ToSmallInt(vlengthColumns.Substring(cont, 4))*2)) -
                                   (vLengthOffset + 12));
                    cont = cont + 4;
                }

                //get varlength values
                for (var i = 0; i < vlengthn; i++)
                {
                    if (i != vlengthn - 1)
                    {

                        var toAdd =
                            Conversions.Converter(
                                vlengthColumns.Substring(delimiters[i], delimiters[i + 1] - (delimiters[i])),
                                metadata.ElementAt(metadata.Count - vlengthn + i).Type);
                        result.Add(toAdd);
                    }
                    else
                    {
                        var toAdd = Conversions.Converter(vlengthColumns.Substring(delimiters[i]),
                            metadata.ElementAt(metadata.Count - 1).Type);
                        result.Add(toAdd);
                    }
                }
            }
            else
            {
              

                rowlogcontentzero = rowlogcontentzero.Substring(8);

                    for (int i = 0, n = 0; i < metadata.Count; i++)
                    {
                        result.Add(Conversions.Converter(rowlogcontentzero.Substring(n, metadata.ElementAt(i).Len*2),
                            metadata.ElementAt(i).Type));
                        n += metadata.ElementAt(i).Len*2;
                    }
                
            }
            return result;
        }

        public static int CantidadLongitudFija(string rowlogcontentzero, List<MetaData> metadata)
        {
            var vlengthn =-1;
            if (rowlogcontentzero.StartsWith("3"))
            {

                var vLengthOffset = (Conversions.ToSmallInt(rowlogcontentzero.Substring(4, 4))*2) - 2;

                //remove the first 4 bytes
                rowlogcontentzero = rowlogcontentzero.Substring(8);

                //get the number of varlength columns
                 vlengthn = Conversions.ToSmallInt(rowlogcontentzero.Substring(vLengthOffset, 4));
            }
            return vlengthn;
        } 

    }
}
