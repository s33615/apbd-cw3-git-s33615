using System;

namespace WypozyczalniaApp.Domain
{
    public abstract class Equipment
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsAvailable { get; set; } = true;

        protected Equipment(string name)
        {
            Name = name;
        }
    }

    public class Laptop : Equipment
    {
        public int RamSizeGb { get; set; }
        public string ProcessorType { get; set; }

        public Laptop(string name, int ramSizeGb, string processorType) : base(name)
        {
            RamSizeGb = ramSizeGb;
            ProcessorType = processorType;
        }
    }

    public class Projector : Equipment
    {
        public string Resolution { get; set; }
        public int Lumens { get; set; }

        public Projector(string name, string resolution, int lumens) : base(name)
        {
            Resolution = resolution;
            Lumens = lumens;
        }
    }

    public class Camera : Equipment
    {
        public double Megapixels { get; set; }
        public string LensType { get; set; }

        public Camera(string name, double megapixels, string lensType) : base(name)
        {
            Megapixels = megapixels;
            LensType = lensType;
        }
    }
}