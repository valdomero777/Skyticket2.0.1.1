﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Skyticket.SkyticketWebService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tickets.ws/", ConfigurationName="SkyticketWebService.ws_tickets_in")]
    public interface ws_tickets_in {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tickets.ws/ws_tickets_in/ingresa_ticketsRequest", ReplyAction="http://tickets.ws/ws_tickets_in/ingresa_ticketsResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Skyticket.SkyticketWebService.ingresa_ticketsResponse ingresa_tickets(Skyticket.SkyticketWebService.ingresa_ticketsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tickets.ws/ws_tickets_in/ingresa_ticketsRequest", ReplyAction="http://tickets.ws/ws_tickets_in/ingresa_ticketsResponse")]
        System.Threading.Tasks.Task<Skyticket.SkyticketWebService.ingresa_ticketsResponse> ingresa_ticketsAsync(Skyticket.SkyticketWebService.ingresa_ticketsRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tickets.ws/")]
    public partial class ctTicketsBeans : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string emailField;
        
        private int id_clienteField;
        
        private int id_terminalField;
        
        private byte[] imagenField;
        
        private string mobilePhoneField;
        
        private string nombreField;
        
        private string passwField;
        
        private string printMethodField;
        
        private string sentField;
        
        private string ticket_contentField;
        
        private string tipoField;
        
        private string usuarioField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string email {
            get {
                return this.emailField;
            }
            set {
                this.emailField = value;
                this.RaisePropertyChanged("email");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public int id_cliente {
            get {
                return this.id_clienteField;
            }
            set {
                this.id_clienteField = value;
                this.RaisePropertyChanged("id_cliente");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public int id_terminal {
            get {
                return this.id_terminalField;
            }
            set {
                this.id_terminalField = value;
                this.RaisePropertyChanged("id_terminal");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary", Order=3)]
        public byte[] imagen {
            get {
                return this.imagenField;
            }
            set {
                this.imagenField = value;
                this.RaisePropertyChanged("imagen");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string mobilePhone {
            get {
                return this.mobilePhoneField;
            }
            set {
                this.mobilePhoneField = value;
                this.RaisePropertyChanged("mobilePhone");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string nombre {
            get {
                return this.nombreField;
            }
            set {
                this.nombreField = value;
                this.RaisePropertyChanged("nombre");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string passw {
            get {
                return this.passwField;
            }
            set {
                this.passwField = value;
                this.RaisePropertyChanged("passw");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string printMethod {
            get {
                return this.printMethodField;
            }
            set {
                this.printMethodField = value;
                this.RaisePropertyChanged("printMethod");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string sent {
            get {
                return this.sentField;
            }
            set {
                this.sentField = value;
                this.RaisePropertyChanged("sent");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string ticket_content {
            get {
                return this.ticket_contentField;
            }
            set {
                this.ticket_contentField = value;
                this.RaisePropertyChanged("ticket_content");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string tipo {
            get {
                return this.tipoField;
            }
            set {
                this.tipoField = value;
                this.RaisePropertyChanged("tipo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string usuario {
            get {
                return this.usuarioField;
            }
            set {
                this.usuarioField = value;
                this.RaisePropertyChanged("usuario");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ingresa_tickets", WrapperNamespace="http://tickets.ws/", IsWrapped=true)]
    public partial class ingresa_ticketsRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tickets.ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("ticket", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Skyticket.SkyticketWebService.ctTicketsBeans[] ticket;
        
        public ingresa_ticketsRequest() {
        }
        
        public ingresa_ticketsRequest(Skyticket.SkyticketWebService.ctTicketsBeans[] ticket) {
            this.ticket = ticket;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ingresa_ticketsResponse", WrapperNamespace="http://tickets.ws/", IsWrapped=true)]
    public partial class ingresa_ticketsResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tickets.ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string @return;
        
        public ingresa_ticketsResponse() {
        }
        
        public ingresa_ticketsResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ws_tickets_inChannel : Skyticket.SkyticketWebService.ws_tickets_in, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ws_tickets_inClient : System.ServiceModel.ClientBase<Skyticket.SkyticketWebService.ws_tickets_in>, Skyticket.SkyticketWebService.ws_tickets_in {
        
        public ws_tickets_inClient() {
        }
        
        public ws_tickets_inClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ws_tickets_inClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ws_tickets_inClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ws_tickets_inClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Skyticket.SkyticketWebService.ingresa_ticketsResponse ingresa_tickets(Skyticket.SkyticketWebService.ingresa_ticketsRequest request) {
            return base.Channel.ingresa_tickets(request);
        }
        
        public System.Threading.Tasks.Task<Skyticket.SkyticketWebService.ingresa_ticketsResponse> ingresa_ticketsAsync(Skyticket.SkyticketWebService.ingresa_ticketsRequest request) {
            return base.Channel.ingresa_ticketsAsync(request);
        }
    }
}
