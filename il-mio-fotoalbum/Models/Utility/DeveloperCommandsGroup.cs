namespace il_mio_fotoalbum.Models.Utility
{
    public class DeveloperCommandsGroup
    {
        public string Name { get; set; }
        public List<DeveloperCommandModel> Commands { get; set; } = new List<DeveloperCommandModel>();

        public DeveloperCommandsGroup(string Name, List<DeveloperCommandModel> DeveloperCommands)
        {
            this.Name = Name;
            Commands = DeveloperCommands;
        }

        public void EmptyCommandsResponsesAndExceptions()
        {
            foreach (var command in Commands)
            {
                command.EmptyResponseAndException();
            }
        }

        public DeveloperCommandModel? GetDeveloperCommandModel(Guid commandId)
        {
            foreach (var command in Commands)
            {
                if (command.DeveloperCommandModelGuid == commandId)
                    return command;
            }
            return null;
        }

    }
}
