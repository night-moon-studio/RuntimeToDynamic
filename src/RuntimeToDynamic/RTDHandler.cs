namespace Natasha.RuntimeToDynamic
{

    public class RTDHandler
    {


        public virtual AssemblyDomain Domain
        {
            get { return DomainManagment.Default; }
            set { }
        }




        public virtual string Namespace
        {
            get { return default; }
            set { }
        }




        public NDomain DelegateHandler
        {
            get { return NDomain.Create(Domain).Using(Namespace); }
        }
        public NClass ClassHandler
        {
            get { return NClass.Create(Domain).Using(Namespace); }
        }
        public NEnum EnumHandler
        {
            get { return NEnum.Create(Domain).Using(Namespace); }
        }
        public NInterface InterfaceHandler
        {
            get { return NInterface.Create(Domain).Using(Namespace); }
        }
        public NStruct StructHandler
        {
            get { return NStruct.Create(Domain).Using(Namespace); }
        }

    }

}
