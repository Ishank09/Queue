namespace TakeHome.Implementation.Models
{

    public class NodeWrapper<T>
    {
        public T data;
        public NodeWrapper<T> next;
        public NodeWrapper(T item)
        {
            this.data = item;
            this.next = null;
        }
    }
}

