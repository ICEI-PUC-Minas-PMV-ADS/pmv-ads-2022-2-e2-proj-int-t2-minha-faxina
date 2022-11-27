namespace PucWebApplication.Models {
    public class ListagemPerfil {  
        
        public Usuario oUsuario { get; set; }
        public ListagemPerfil(Usuario oUsuario) {
            this.oUsuario = oUsuario;
        }
        public IEnumerable<Usuario> Lista {get;set;}


    }
}
