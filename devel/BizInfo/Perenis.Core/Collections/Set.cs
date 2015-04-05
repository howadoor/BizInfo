using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Datový typ množina
    /// </summary>
    /// <typeparam name="T">Typ prvků množiny</typeparam>
    [Serializable]
    public class Set<T> : ICollectionEx<T>, IEquatable<ICollection<T>>, ICloneable, IFormattable
    {
        #region ------ Konstruktory ---------------------------------------------------------------

        /// <summary>
        /// Implicitní konstruktor, vytvoří prázdnou množinu
        /// </summary>
        public Set()
        {
            data = new Dictionary<T, int>();
        }

        /// <summary>
        /// Kopírující konstruktor, vytvoří množinu obsahující právě dodané prvky
        /// </summary>
        /// <param name=Constants.ItemsName>Počáteční naplnění množiny</param>
        public Set(ICollection<T> items)
        {
            data = new Dictionary<T, int>(items.Count);
            foreach (T item in items) Add(item);
        }

        /// <summary>
        /// Kopírující konstruktor, vytvoří množinu obsahující právě dodané prvky
        /// </summary>
        /// <param name=Constants.ItemsName>Počáteční naplnění množiny</param>
        public Set(IEnumerable<T> items)
        {
            data = new Dictionary<T, int>();
            foreach (T item in items) Add(item);
        }

        /// <summary>
        /// Vytvoří prázdnou množinu s rezervovanou kapacitou pro udaný počet prvků
        /// </summary>
        /// <param name="capacity">Kapacita (počet prvků)</param>
        public Set(int capacity)
        {
            data = new Dictionary<T, int>(capacity);
        }

        #endregion

        #region ------ Veřejné metody -------------------------------------------------------------

        private bool isReadOnly;

        #region ICloneable Members

        /// <summary>
        /// Vytvoří mělkou kopii této množiny
        /// </summary>
        /// <returns>Nový objekt, odkazující na stejné objekty jako tato množina</returns>
        public object Clone()
        {
            return new Set<T>(this);
        }

        #endregion

        #region ICollectionEx<T> Members

        /// <summary>
        /// Přidá do této množiny jeden prvek
        /// </summary>
        /// <param name="item">Přidávaný prvek</param>
        public virtual void Add(T item)
        {
            if (IsReadOnly) throw new NotSupportedException("This collection is read-only");
            data[item] = 1;
        }

        /// <summary>
        /// Adds the elements of the specified collection to the current collection.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the current collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is a null reference.</exception>
        /// <exception cref="NotSupportedException">When the current collection is read-only.</exception>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (IsReadOnly) throw new NotSupportedException("This collection is read-only");
            foreach (T item in collection) Add(item);
        }

        /// <summary>
        /// Vyjme z této množiny jeden prvek
        /// </summary>
        /// <param name="item">Odebíraný prvek</param>
        /// <returns>true pokud byl prvek odebrán, false pokud prvek v množině nebyl</returns>
        public bool Remove(T item)
        {
            if (IsReadOnly) throw new NotSupportedException("This collection is read-only");
            return data.Remove(item);
        }

        /// <summary>
        /// Zjistí, zda je daný objekt prvkem této množiny
        /// </summary>
        /// <param name="item">Testovaný objekt</param>
        /// <returns>true pokud je objekt prvkem této množiny</returns>
        public bool Contains(T item)
        {
            return data.ContainsKey(item);
        }

        /// <summary>
        /// Odstraní z této množiny všechny prvky
        /// </summary>
        public void Clear()
        {
            if (IsReadOnly) throw new NotSupportedException("This collection is read-only");
            data.Clear();
        }

        /// <summary>
        /// Zkopíruje obsah této množiny do pole
        /// </summary>
        /// <param name="array">Cílové pole, musí být vytvořeno a mít dostatečnou kapacitu</param>
        /// <param name="arrayIndex">Index, od kterého má být do pole ukládáno</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = 0;
            foreach (T item in this)
            {
                array[arrayIndex + i++] = item;
            }
        }

        /// <summary>
        /// Počet prvků této množiny
        /// </summary>
        public int Count
        {
            get { return data.Count; }
        }

        /// <summary>
        /// Příznak, že tato kolekce je read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                if (isReadOnly) throw new NotSupportedException("This collection is read-only");
                isReadOnly = value;
            }
        }

        /// <summary>
        /// Získá enumerátor přes všechny prvky této množiny
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return data.Keys.GetEnumerator();
        }

        /// <summary>
        /// Získá enumerátor přes všechny prvky této množiny
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEquatable<ICollection<T>> Members

        /// <summary>
        /// Zjistí, zda tato množina obsahuje stejné prvky jako jiná kolekce (rovnost množin)
        /// </summary>
        /// <returns>true, pokud <paramref name="other"/> obsahuje stejné prvky jako tato množina</returns>
        public bool Equals(ICollection<T> other)
        {
            if (other == null) return false;

            if (other.Count != Count) return false;

            foreach (T member in this)
            {
                if (!other.Contains(member)) return false;
            }
            return true;
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Vytvoří textovou reprezentaci této množiny
        /// </summary>
        /// <param name="format">Formátovací řetězec pro jednotlivé prvky</param>
        /// <param name="formatProvider">Poskytovatel formátování pro jednotlivé prvky</param>
        /// <returns>"{a; b; c; d}</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var result = new StringBuilder();
            result.Append('{');
            bool first = true;
            string separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator + " ";
            foreach (T member in this)
            {
                if (!first) result.Append(separator);
                first = false;
                var mf = member as IFormattable;
                result.Append(mf == null ? member.ToString() : mf.ToString(format, formatProvider));
            }
            result.Append('}');
            return result.ToString();
        }

        #endregion

        /// <summary>
        /// Vyjme z této množiny kolekci prvků (množinový rozdíl)
        /// </summary>
        /// <param name=Constants.ItemsName>Odebírané prvky</param>
        public void Remove(ICollection<T> items)
        {
            if (IsReadOnly) throw new NotSupportedException("This collection is read-only");
            foreach (T item in items) Remove(item);
        }

        /// <summary>
        /// Tuto množinu omezí na průnik s jinou množinou
        /// </summary>
        /// <param name="other">Množina, se kterou má být tato množina proniknuta</param>
        public void Intersect(ICollection<T> other)
        {
            if (IsReadOnly) throw new NotSupportedException("This collection is read-only");
            var needRemoval = new List<T>(Count);
            foreach (T member in this)
            {
                if (!other.Contains(member)) needRemoval.Add(member);
            }
            foreach (T removed in needRemoval)
            {
                Remove(removed);
            }
        }

        /// <summary>
        /// Převede tuto množinu na pole
        /// </summary>
        /// <returns>Pole všech prvků této množiny</returns>
        public T[] ToArray()
        {
            var result = new T[Count];
            data.Keys.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Zjistí, zda tato množina obsahuje stejné prvky jako jiná kolekce
        /// </summary>
        /// <param name="obj">Porovnávaná kolekce, musí implementovat ICollection<typeparamref name="T"/></param>
        /// <returns>true, pokud je <paramref name="obj"/> kolekce a obsahuje stejné prvky jako tato množina</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ICollection<T>);
        }

        /// <summary>
        /// Získá hash této množiny
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (T member in this)
            {
                hash ^= member.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Vytvoří implicitně formátovanou textovou reprezentaci této množiny
        /// </summary>
        /// <returns>"{a; b; c; d}"</returns>
        public override string ToString()
        {
            return ToString(null, null);
        }

        #endregion

        #region ------ Přetížené operátory --------------------------------------------------------

        /// <summary>
        /// Rovnost množin
        /// </summary>
        public static bool operator ==(Set<T> a, ICollection<T> b)
        {
            if (ReferenceEquals(a, null)) return ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>
        /// Nerovnost množin
        /// </summary>
        public static bool operator !=(Set<T> a, ICollection<T> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Sjednocení množin
        /// </summary>
        public static Set<T> operator +(Set<T> a, Set<T> b)
        {
            var result = new Set<T>(a);
            result.AddRange(b);
            return result;
        }

        /// <summary>
        /// Vložení prvku do množiny
        /// </summary>
        public static Set<T> operator +(Set<T> set, T item)
        {
            var result = new Set<T>(set);
            result.Add(item);
            return result;
        }

        /// <summary>
        /// Množinový rozdíl
        /// </summary>
        public static Set<T> operator -(Set<T> a, Set<T> b)
        {
            var result = new Set<T>(a);
            result.Remove(b);
            return result;
        }

        /// <summary>
        /// Vyjmutí prvku z množiny
        /// </summary>
        public static Set<T> operator -(Set<T> set, T item)
        {
            var result = new Set<T>(set);
            result.Remove(item);
            return result;
        }

        /// <summary>
        /// Průnik množin
        /// </summary>
        public static Set<T> operator *(Set<T> a, Set<T> b)
        {
            var result = new Set<T>(a);
            result.Intersect(b);
            return result;
        }

        #endregion

        #region ------ Data -----------------------------------------------------------------------

        /// <summary>
        /// Data uložená v této množině.
        /// </summary>
        /// <remarks>
        /// Množina je reprezentována pomocí hashtabulky, prvky jsou klíče, příslušné hodnoty jsou
        /// nepodstatné.
        /// </remarks>
        private readonly Dictionary<T, int> data;

        #endregion
    }
}