using Bogus;
using System;

namespace HoleDriven.Bogus
{
    public static class ProvideValueExtensions
    {
        public static TValue Bogus<TValue>(this Hole.IProvideInput _, Func<Faker, TValue> fake, string locale = "en")
        {
            return fake(new Faker(locale));
        }

        public static TValue Bogus<TValue>(this Hole.IProvideInput _, Func<Faker<TValue>, Faker<TValue>> fake, string locale = "en", IBinder binder = null)
            where TValue : class
        {
            return fake(new Faker<TValue>(locale, binder)).Generate();
        }
    }
}
