﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Skyticket.ClientValidationService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://servicios.tickets.com/", ConfigurationName="ClientValidationService.ws_valida_cliente")]
    public interface ws_valida_cliente {
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://servicios.tickets.com/ws_valida_cliente/respuestaRequest", ReplyAction="http://servicios.tickets.com/ws_valida_cliente/respuestaResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        Skyticket.ClientValidationService.respuestaResponse respuesta(Skyticket.ClientValidationService.respuestaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://servicios.tickets.com/ws_valida_cliente/respuestaRequest", ReplyAction="http://servicios.tickets.com/ws_valida_cliente/respuestaResponse")]
        System.Threading.Tasks.Task<Skyticket.ClientValidationService.respuestaResponse> respuestaAsync(Skyticket.ClientValidationService.respuestaRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://servicios.tickets.com/")]
    public partial class ctClienteBeans : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int id_clienteField;
        
        private bool id_clienteFieldSpecified;
        
        private int id_terminalField;
        
        private bool id_terminalFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool id_clienteSpecified {
            get {
                return this.id_clienteFieldSpecified;
            }
            set {
                this.id_clienteFieldSpecified = value;
                this.RaisePropertyChanged("id_clienteSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool id_terminalSpecified {
            get {
                return this.id_terminalFieldSpecified;
            }
            set {
                this.id_terminalFieldSpecified = value;
                this.RaisePropertyChanged("id_terminalSpecified");
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
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="respuesta", WrapperNamespace="http://servicios.tickets.com/", IsWrapped=true)]
    public partial class respuestaRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://servicios.tickets.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Skyticket.ClientValidationService.ctClienteBeans consulta;
        
        public respuestaRequest() {
        }
        
        public respuestaRequest(Skyticket.ClientValidationService.ctClienteBeans consulta) {
            this.consulta = consulta;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="respuestaResponse", WrapperNamespace="http://servicios.tickets.com/", IsWrapped=true)]
    public partial class respuestaResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://servicios.tickets.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string @return;
        
        public respuestaResponse() {
        }
        
        public respuestaResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ws_valida_clienteChannel : Skyticket.ClientValidationService.ws_valida_cliente, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ws_valida_clienteClient : System.ServiceModel.ClientBase<Skyticket.ClientValidationService.ws_valida_cliente>, Skyticket.ClientValidationService.ws_valida_cliente {
        
        public ws_valida_clienteClient() {
        }
        
        public ws_valida_clienteClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ws_valida_clienteClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ws_valida_clienteClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ws_valida_clienteClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Skyticket.ClientValidationService.respuestaResponse Skyticket.ClientValidationService.ws_valida_cliente.respuesta(Skyticket.ClientValidationService.respuestaRequest request) {
            return base.Channel.respuesta(request);
        }
        
        public string respuesta(Skyticket.ClientValidationService.ctClienteBeans consulta) {
            Skyticket.ClientValidationService.respuestaRequest inValue = new Skyticket.ClientValidationService.respuestaRequest();
            inValue.consulta = consulta;
            Skyticket.ClientValidationService.respuestaResponse retVal = ((Skyticket.ClientValidationService.ws_valida_cliente)(this)).respuesta(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Skyticket.ClientValidationService.respuestaResponse> Skyticket.ClientValidationService.ws_valida_cliente.respuestaAsync(Skyticket.ClientValidationService.respuestaRequest request) {
            return base.Channel.respuestaAsync(request);
        }
        
        public System.Threading.Tasks.Task<Skyticket.ClientValidationService.respuestaResponse> respuestaAsync(Skyticket.ClientValidationService.ctClienteBeans consulta) {
            Skyticket.ClientValidationService.respuestaRequest inValue = new Skyticket.ClientValidationService.respuestaRequest();
            inValue.consulta = consulta;
            return ((Skyticket.ClientValidationService.ws_valida_cliente)(this)).respuestaAsync(inValue);
        }
    }
}
