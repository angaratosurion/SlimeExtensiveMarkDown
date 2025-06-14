# SlimeMarkUp Cheat Sheet

**SlimeMarkUp** είναι μια βιβλιοθήκη σε C# για parsing και εξαγωγή markup σε HTML. Παρακάτω θα βρεις σύντομες οδηγίες και παραδείγματα για βασικά blocks και επεκτάσεις.

---

## Headers (Κεφαλίδες)
- Ξεκινούν με ένα ή περισσότερα `#`.
- Ο αριθμός των `#` καθορίζει το επίπεδο της κεφαλίδας (`h1`, `h2` κ.λπ.).
- **Παράδειγμα:**
  ```
  # Κεφαλίδα 1
  ## Κεφαλίδα 2
  ```

## Λίστες (Lists)
- Κάθε στοιχείο λίστας ξεκινά με `- `.
- Γράψε πολλά στοιχεία στη σειρά.
- **Παράδειγμα:**
  ```
  - στοιχείο 1
  - στοιχείο 2
  ```
- Μετατρέπεται σε HTML:
  ```html
  <ul>
    <li>στοιχείο 1</li>
    <li>στοιχείο 2</li>
  </ul>
  ```

## Κώδικας (Code Blocks)
- Οριοθέτησε τον κώδικα με τριπλά backticks: ```
- **Παράδειγμα:**
  ```
  ```
  Console.WriteLine("Hello, SlimeMarkUp!");
  ```
  ```

- Παράγει HTML:
  ```html
  <pre>
  Console.WriteLine("Hello, SlimeMarkUp!");
  </pre>
  ```

## Inline Styles (Ενσωματωμένα στυλ)
- Υπάρχουν επεκτάσεις για inline format, όπως bold, italic κ.λπ. (**δες κώδικα για λεπτομέρειες**).
- Συνήθως γίνεται parsing μέσω Regex (δες `InlineStyleExtension.cs`).

---

## Εξαγωγή σε HTML
- Η κλάση `HtmlExporter` μετατρέπει τη λίστα `MarkupElement` σε HTML string.
- Υποστηρίζει tags, attributes, children.
- **Παράδειγμα χρήσης:**
  ```csharp
  var html = new HtmlExporter().Export(elements);
  ```

---

## Επεκτασιμότητα
- Μπορείς να προσθέσεις δικές σου επεκτάσεις υλοποιώντας το `IBlockMarkupExtension`.
- Κάθε επέκταση πρέπει να υλοποιεί τα: `CanParse`, `Parse`, `ParseBlock`.

---

## Προτεινόμενη δομή κώδικα για επέκταση:
```csharp
public class CustomBlockExtension : IBlockMarkupExtension
{
    public bool CanParse(string line) { ... }
    public MarkupElement? Parse(string line) { ... }
    public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines) { ... }
}
```

---

## Άδεια Χρήσης
- GNU GPLv3.

---

### Για περισσότερα, δες τα αρχεία στον φάκελο `SlimeMarkUp.Core/Extensions`.
