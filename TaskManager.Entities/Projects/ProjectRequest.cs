using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Entities.Projects
{
    public class ProjectRequest
    {
        // Clave primaria del proyecto
        public int IdProject { get; set; }

        // Nombre del proyecto
        public string ProjectName { get; set; } = string.Empty;

        // Descripción del proyecto
        public string ProjectDescription { get; set; } = string.Empty;

        // Fecha de inicio del proyecto
        public DateTime StartDate { get; set; }

        // Fecha de finalización del proyecto (puede ser null)
        public DateTime? EndDate { get; set; }

        // Estado actual del proyecto (ej. Activo, Completado, etc.)
        public string Status { get; set; } = string.Empty;

        // Prioridad del proyecto (Alta, Media, Baja)
        public string Priority { get; set; } = string.Empty;

        // ID del usuario que gestiona el proyecto
        public int IdManager { get; set; }

        // Presupuesto asignado al proyecto (puede ser null)
        public decimal? Budget { get; set; }

        // Fecha de creación del proyecto
        public DateTime CreationDate { get; set; }

        // Fecha de última modificación del proyecto
        public DateTime ModificationDate { get; set; }

        // Indicador de si el proyecto está archivado
        public bool IsArchived { get; set; }

        // Usuario que creó el proyecto
        public int UserCreation { get; set; }

        // Usuario que modificó el proyecto por última vez
        public int? UserModification { get; set; }
    }
}
