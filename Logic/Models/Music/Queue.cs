using Discord;
using Logic.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Models.Music
{
    public class Queue
    {
        private Dictionary<ulong, Queue<IPlayable>> _queues;

        public Queue<IPlayable> this[IGuild guild] => GetQueue(guild);

        public Queue()
        {
            _queues = new Dictionary<ulong, Queue<IPlayable>>();
        }

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

        public void Enqueue(IPlayable item)
        {
            var guild = item.Guild;
            var queue = GetQueue(guild);
            queue.Enqueue(item);
        }

        public IPlayable Dequeue(IGuild guild)
        {
            var queue = GetQueue(guild);
            if (!queue.Any()) throw new InvalidOperationException("There are no items remaining in the queue.");
            return queue.Dequeue();
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