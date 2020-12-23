using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 21)]
    class Year2020Day21 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var foods = ParseInput(input)
                .ToList();

            var allIngredients = new HashSet<string>(foods.SelectMany(f => f.ingredients));
            var allAllergens = new HashSet<string>(foods.SelectMany(f => f.allergens));
            var allergenLookup = GetAllergenLookup(foods);
            List<string> safeIngredients = GetSafeIngredients(allIngredients, allAllergens, allergenLookup);

            var occurenceOfSafeIngredients = safeIngredients.Sum(i => foods.Count(f => f.ingredients.Contains(i)));
            return $"{occurenceOfSafeIngredients}";
        }

        private static List<string> GetSafeIngredients(HashSet<string> allIngredients, HashSet<string> allAllergens, Dictionary<string, List<List<string>>> allergenLookup)
        {
            var safeIngredients = new List<string>();
            foreach (var ingredient in allIngredients)
            {
                var isSafe = true;
                foreach (var allergen in allAllergens)
                {
                    if (allergenLookup[allergen].All(f => f.Contains(ingredient)))
                    {
                        isSafe = false;
                        break;
                    }
                }

                if (isSafe)
                {
                    safeIngredients.Add(ingredient);
                }
            }

            return safeIngredients;
        }

        public string Part2(IList<string> input)
        {
            var foods = ParseInput(input)
                .ToList();

            var allIngredients = new HashSet<string>(foods.SelectMany(f => f.ingredients));
            var allAllergens = new HashSet<string>(foods.SelectMany(f => f.allergens));
            var allergenLookup = GetAllergenLookup(foods);
            var safeIngredients = new HashSet<string>(GetSafeIngredients(allIngredients, allAllergens, allergenLookup));
            var unSafeIngredients = new HashSet<string>(allIngredients);
            unSafeIngredients.ExceptWith(safeIngredients);
            var unmappedAllergens = new HashSet<string>(allAllergens);

            var allergentList = new List<(string allergen, string ingredient)>();
            var listKnownUnsafeIngredient = new HashSet<string>();
            while (unmappedAllergens.Count > 0)
            {
                foreach (var allergen in unmappedAllergens.ToList())
                {
                    var dangerousFoods = allergenLookup[allergen];
                    var ingredients = new HashSet<string>(
                        dangerousFoods.SelectMany(df => df)
                        .Where(i => unSafeIngredients.Contains(i))
                        .Where(i => !listKnownUnsafeIngredient.Contains(i)));

                    var potentialIngredients = ingredients
                        .Where(i => 
                            allergenLookup[allergen].All(f => f.Contains(i)));

                    if (potentialIngredients.Count() == 1)
                    {
                        allergentList.Add((allergen, potentialIngredients.First()));
                        unmappedAllergens.Remove(allergen);
                        listKnownUnsafeIngredient.Add(potentialIngredients.First());
                    }
                }
            }

            return string.Join(",", allergentList
                .OrderBy(a => a.allergen)
                .Select(a => a.ingredient));
        }

        internal Dictionary<string, List<List<string>>> GetAllergenLookup(List<(List<string> ingredients, List<string> allergens)> foods)
        {
            var result = new Dictionary<string, List<List<string>>>();
            foreach (var food in foods)
            {
                foreach (var allergen in food.allergens)
                {
                    if (!result.ContainsKey(allergen))
                    {
                        result[allergen] = new List<List<string>>();
                    }

                    result[allergen].Add(food.ingredients);
                }
            }

            return result;
        }

        internal IEnumerable<(List<string> ingredients, List<string> allergens)> ParseInput(IList<string> input)
        {
            var matcher = new Regex(@"((?<ingredient>\w+) )+\(contains ((?<allergen>\w+),? ?)+\)");
            foreach (var line in input)
            {
                var match = matcher.Match(line);
                var ingredients = match.Groups["ingredient"].Captures.Select(c => c.Value).ToList();
                var allergens = match.Groups["allergen"].Captures.Select(c => c.Value).ToList();
                yield return (ingredients, allergens);
            }
        }
    }
}
