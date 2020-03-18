namespace Logic.Models.Music
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Discord;
    using Extensions;

    public class Queue
    {
        private readonly Dictionary<ulong, Queue<IPlayable>> _queues;

        public Queue()
        {
            _queues = new Dictionary<ulong, Queue<IPlayable>>();
        }

        public Queue<IPlayable> this[IGuild guild] => GetQueue(guild);

        public Queue<IPlayable> GetQueue(IGuild guild)
        {
            if (!_queues.TryGetValue(guild.Id, out var queue))
            {
                queue = new Queue<IPlayable>();
                _queues.Add(guild.Id, queue);
            }

            return queue;
        }

        public int Count(IGuild guild)
        {
            var queue = GetQueue(guild);
            return queue.Count;
        }

        public bool HasItems(IGuild guild)
        {
            var queue = GetQueue(guild);
            return queue.Any();
        }

        public int Enqueue(IPlayable item)
        {
            var guild = item.Guild;
            var queue = GetQueue(guild);
            queue.Enqueue(item);
            return queue.Count;
        }

        public int Enqueue(IEnumerable<IPlayable> items)
        {
            var guild = items.First().Guild;
            var queue = GetQueue(guild);
            var index = queue.Count + 1;
            items.Foreach(x => queue.Enqueue(x));
            return index;
        }

        public IPlayable Dequeue(IGuild guild)
        {
            var queue = GetQueue(guild);
            if (!queue.Any()) throw new InvalidOperationException("There are no items remaining in the queue.");
            return queue.Dequeue();
        }

        public void Remove(IGuild guild, int amount = 1)
        {
            var queue = GetQueue(guild);
            if (!queue.Any()) throw new InvalidOperationException("There are no items remaining in the queue.");
            if (queue.Count < amount) amount = queue.Count;
            for (var i = 0; i < amount; i++) queue.Dequeue();
        }

        public void Shuffle(IGuild guild)
        {
            var queue = GetQueue(guild);
            if (!queue.Any()) throw new InvalidOperationException("There are no items remaining in the queue.");
            queue.Shuffle();
        }

        public void Clear(IGuild guild)
        {
            var queue = GetQueue(guild);
            queue.Clear();
        }

        public List<IPlayable> GetItems(IGuild guild)
        {
            var queue = GetQueue(guild);
            return queue.ToList();
        }
    }
}