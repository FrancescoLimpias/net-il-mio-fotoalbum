using System.ComponentModel.DataAnnotations;

namespace il_mio_fotoalbum.Models
{
    public class DeveloperCommandModel
    {

        [Key]
        public Guid DeveloperCommandModelGuid { get; set; } = Guid.NewGuid();

        public string Name { get; set; }
        public bool? Response { get; set; } = null;
        public Exception? Exception { get; set; } = null;

        public Func<bool> Execution { get; set; }

        public DeveloperCommandModel(string Name, Func<bool> Execution)
        {
            this.Name = Name;
            this.Execution = Execution;
        }

        public bool? Execute()
        {
            try
            {
                Response = Execution();
            }
            catch (Exception ex)
            {
                Response = false;
                Exception = ex;
            }
            return Response;
        }

        public void EmptyResponseAndException()
        {
            Response = null;
            Exception = null;
        }

    }
}
