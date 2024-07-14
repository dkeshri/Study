using Dkeshri.SystemDesign.LowLevel.Interfaces;
using System;

namespace Dkeshri.SystemDesign.LowLevel.DesignPrincipal.SOLID
{

    // Open close principal.
    public interface IShape{
        //this Inteface Represent Open for modification/Extension.
        public double Area();
    }
    public class CalculateArea{
        // Close for Modification.
        public double calculateArea(IShape shape){
            return shape.Area();
        }

    }
    public class Circle : IShape
    {
        // here we are Extending the Interface.
        const double PI = 3.14;
        double radius;
        public Circle(double radius){
            this.radius = radius;
        }
        public double Area()
        {
            return 2*PI*radius;
        }
    }
    public class Rectangle:IShape{
        double length;
        double breadth;
        public Rectangle(double length, double breadth){
            this.length = length;
            this.breadth = breadth;
        }

        public double Area()
        {
            return length*breadth;
        }
    }
    public class Triangle : IShape
    {
        double baselen;
        double height;
        public Triangle(double baselen,double height){
            this.baselen = baselen;
            this.height = height;
        }
        public double Area()
        {
            return (baselen*height)/2;
        }
    }



    // here I am calling the Area of different Shape. You can Call it in Main Function.

    public class OpenClosePrincipal : IExecute
    {
        public void run()
        {
            CalculateArea calculateArea = new CalculateArea();
            
            Circle circle = new Circle(3);
            Console.WriteLine("Area of circle of Radius 3 is: "+calculateArea.calculateArea(circle));

            Rectangle rectangle = new Rectangle(5,6);
            Console.WriteLine("Area of rectangle having len=5 and breadth=6 is: "+calculateArea.calculateArea(rectangle));

            Triangle triangle =  new Triangle(5,41);
            Console.WriteLine("Area of triangle is :"+ calculateArea.calculateArea(triangle));
            
        }
    }


}