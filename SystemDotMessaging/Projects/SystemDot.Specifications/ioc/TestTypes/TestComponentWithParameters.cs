namespace SystemDot.Specifications.ioc.TestTypes
{
    class TestComponentWithParameters : ITestComponentWithParameters
    {
        public ITestComponent FirstParameter;
        public IAnotherTestComponent SecondParameter;

        public TestComponentWithParameters(ITestComponent firstParameter, IAnotherTestComponent secondParameter)
        {
            this.FirstParameter = firstParameter;
            this.SecondParameter = secondParameter;
        }
    }
}