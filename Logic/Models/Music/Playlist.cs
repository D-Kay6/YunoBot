using System.Collections.Generic;

namespace Logic.Models.Music
{
    public class Playlist
    {
        private readonly Dictionary<ulong, Queue<IPlayable>> _queues;

        public Playlist()
        {
            _queues = new Dictionary<ulong, Queue<IPlayable>>();
        }

        public Queue<IPlayable> this[ulong id]
        {
            get { return this.GetQueue(id); }
        }

        private Queue<IPlayable> GetQueue(ulong id)
        {
            if (_queues.TryGetValue(id, out var queue)) return queue;
            queue = new Queue<IPlayable>();
            _queues.Add(id, queue);
            return queue;
        }
    }
}
