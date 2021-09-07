using HighlyAvailableMonolithPOC.Infrastructure.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HighlyAvailableMonolithPOC.Folders.Commands
{
    public class CreateFolderCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class CreateFolderCommandHandler : IRequestHandler<CreateFolderCommand, Guid>
    {
        private readonly ApplicationDbContext context;

        public CreateFolderCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
        {
            Guid id = Guid.NewGuid();

            context.Folders.Add(new Folder()
            {
                Id = id,
                DisplayName = request.Name,
                ParentId = request.ParentId
            });

            await context.SaveChangesAsync();

            return id;
        }
    }
}
