using System.Collections.Generic;

[System.Serializable]
public class ListWrapperJSON<T>
{
    public List<T> list;
    public ListWrapperJSON(List<T> list) => this.list = list;
}