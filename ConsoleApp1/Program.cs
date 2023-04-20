using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        #region Scenario 1
        // Read the input JSON file
        string inputJson = File.ReadAllText("C:\\Users\\DELL\\source\\repos\\ConsoleApp1\\ConsoleApp1\\coding-assigment-orders.json");
        Dictionary<string, Dictionary<string, string>> orders = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(inputJson);
        Dictionary<string, Dictionary<string, string>> output = new Dictionary<string, Dictionary<string, string>>();
        
        // Initialize the flights just example right now in JSON we have only 3 Destination 
        List<FlightData> flights = new List<FlightData>();
        flights.Add(new FlightData("YYZ"));
        flights.Add(new FlightData("YYC"));
        flights.Add(new FlightData("YVR"));

        // Assign boxes to flights
        foreach (KeyValuePair<string, Dictionary<string, string>> order in orders)
        {
            string destination = order.Value["destination"];
            bool assigned = false;
            foreach (FlightData flight in flights)
            {
                if (flight.Destination == destination && flight.BoxCount < flight.Capacity)
                {
                    flight.AddBox(order.Key);
                    assigned = true;
                    Dictionary<string, string> BoxShipped = new Dictionary<string, string>
                    {
                        { "flight", flight.Number }, { "destination", flight.Destination }
                    };
                    output.Add(destination + "_" + order.Key, BoxShipped);
                    break;
                }
            }
            if (!assigned)
            {
                output.Add(destination + "_" + order.Key + "_Not Assigned to flight.  ", order.Value);
                Console.WriteLine($"Order {order.Key} could not be assigned to a flight.");
            }
        }

        // Save the output in New JSON file named output.json
        string outputJson = JsonConvert.SerializeObject(output, Formatting.Indented);
        File.WriteAllText("C:\\Users\\DELL\\source\\repos\\ConsoleApp1\\ConsoleApp1\\output.json", outputJson);
        #endregion

        #region Implmentation of User Story 1
        List<Flights> inputFlights = new List<Flights>();
        Flights flightObj = null;
        Console.Clear();
        Console.WriteLine("Enter the number of flights to schedule:");
        int numFlights = int.Parse(Console.ReadLine());
        if (numFlights > 0)

        {
            for (int i = 1; i <= numFlights; i++)
            {
                flightObj = new Flights();
                flightObj.Number = i;

                Console.WriteLine("Enter the departure city for Flight {0}:", i);
                flightObj.Departure = Console.ReadLine();

                Console.WriteLine("Enter the arrival city for Flight {0}:", i);
                flightObj.Arrival = Console.ReadLine();

                Console.WriteLine("Enter the day of the week for Flight {0}:", i);
                flightObj.Day = int.Parse(Console.ReadLine());

                inputFlights.Add(flightObj);
            }
        }
        else
        {
            Console.WriteLine("Scheduled Flights must be greater than {0}:", numFlights);
            Console.ReadLine();
        }
        Console.WriteLine("Loaded flight schedule:");
        foreach (Flights flight in inputFlights)
        {
            Console.WriteLine("Flight: {0}, departure: {1}, arrival: {2}, day: {3}", flight.Number, flight.Departure, flight.Arrival, flight.Day);
        }
        Console.ReadLine();
        #endregion

        #region Implementation of User Story 2
        // Read orders from JSON file
        var jsonString = File.ReadAllText("C:\\Users\\DELL\\source\\repos\\ConsoleApp1\\ConsoleApp1\\coding-assigment-orders.json");
        if (!string.IsNullOrEmpty(jsonString))
        {
            userStory2 obj = new userStory2();

            // Call the function to group the orders by destination
            obj.GroupOrdersByDestination(jsonString);
            Console.ReadLine();
        }
        #endregion
    }
}

public class userStory2
{
    public void GroupOrdersByDestination(string json)
    {
        //Dictionary<string, List<string>> groupedOrders = new Dictionary<string, List<string>>();
        Dictionary<string, FlightDestination> orders = JsonConvert.DeserializeObject<Dictionary<string, FlightDestination>>(json);

        // Set departure and arrival cities for eamples I took arrival as Montreal
        string departure_city = "YYZ";
        string arrival_city = "Montreal";

        // Initialize flight number and day
        int flightNumber = 1;
        int day = 1;

        // Schedule orders
        foreach (KeyValuePair<string, FlightDestination> order in orders)
        {
            // If destination is different, assign new flight number and day
            if (order.Value.destination != arrival_city)
            {
                flightNumber++;
                day = 1;
                arrival_city = order.Value.destination;
            }

            // If flight number is greater than 99, exit the loop
            if (flightNumber > 99)
            {
                break;
            }

            // Schedule order
            Console.WriteLine("order: " + order.Key + ", flightNumber: " + flightNumber + ", departure: " + departure_city + ", arrival: " + arrival_city + ", day: " + day);
            day++;
        }

        // Output not yet scheduled orders
        while (flightNumber <= 99)
        {
            Console.WriteLine("order: order-" + flightNumber.ToString("000") + ", flightNumber: not scheduled");
            flightNumber++;
        }
    }

}


public class FlightData
{
    public string Number { get; }
    public string Destination { get; }
    public int Capacity { get; } = 20;
    public int BoxCount { get { return Boxes.Count; } }
    public List<string> Boxes { get; } = new List<string>();

    public FlightData(string destination)
    {
        Number = $"Flight {destination}";
        Destination = destination;
    }

    public void AddBox(string box)
    {
        Boxes.Add(box);
    }
}

public class Flights
{
    public int Number { get; set; }
    public string Departure { get; set; }
    public string Arrival { get; set; }
    public int Day { get; set; }
}

public class Order
{
    public string Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public int Priority { get; set; }
}

public class FlightItinerary
{
    public Order Order { get; set; }
    public Flights Flight { get; set; }

    public FlightItinerary(Order order, Flights flight)
    {
        Order = order;
        Flight = flight;
    }
}

public class FlightDestination
{
    public string destination { get; set; }
}
