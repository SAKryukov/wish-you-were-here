namespace SA.Main {
    using SA.Univeral.Utilities;
    using static System.Console;
    
    static class Test {

        static void Main() {            
            if (FirstTime.Here) {
                System.Console.WriteLine(System.Reflection.Assembly.GetEntryAssembly().Location);
                Main();
            }
        } //Main

    } //class Test

}
