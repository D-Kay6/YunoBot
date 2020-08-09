using Core.Enum;
using Discord.Commands;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Logic.Commands.TypeReaders
{
    public class AutomationTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Task.FromResult(TypeReaderResult.FromError(new InvalidDataException("The input was empty.")));

            switch (input.ToLower())
            {
                case "0":
                case "temp":
                case "temporary":
                case "auto":
                case "automatic":
                    return Task.FromResult(TypeReaderResult.FromSuccess(AutomationType.Temporary));
                case "1":
                case "perm":
                case "perma":
                case "permanent":
                    return Task.FromResult(TypeReaderResult.FromSuccess(AutomationType.Permanent));
                default:
                    return Task.FromResult(TypeReaderResult.FromError(new InvalidDataException("The input did not match any of the values.")));
            }
        }
    }
}