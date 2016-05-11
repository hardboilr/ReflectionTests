namespace UnknownObjects.Model {

    public class Person {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Person() { }

        public Person(string firstName, string lastName, int age) {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        public string Hello(string text) {
            return text + " "+ FirstName + " " + LastName;
        }

        public override string ToString() {
            return FirstName + " " + LastName + ", " + Age;
        }
    }
}
