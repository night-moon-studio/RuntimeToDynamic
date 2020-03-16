namespace Natasha.RuntimeToDynamic
{

    public class NHandler
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




        public NDomain NDomainHandler
        {
            get { return NDomain.Create(Domain); }
        }
        public NClass NClassHandler
        {
            get { return NClass.Create(Domain).Using(Namespace); }
        }
        public NEnum NEnumHandler
        {
            get { return NEnum.Create(Domain).Using(Namespace); }
        }
        public NInterface NInterfaceHandler
        {
            get { return NInterface.Create(Domain).Using(Namespace); }
        }
        public NStruct NStructHandler
        {
            get { return NStruct.Create(Domain).Using(Namespace); }
        }

    }

}
