# SlimeMarkUp: Markup → HTML Mapping

Σε αυτό το αρχείο θα βρεις παραδείγματα markup και το αντίστοιχο HTML που παράγει το SlimeMarkUp.

---

## Headers (Κεφαλίδες)

**Markup:**
```
# Κεφαλίδα 1
## Κεφαλίδα 2
```

**HTML:**
```html
<h1>Κεφαλίδα 1</h1>
<h2>Κεφαλίδα 2</h2>
```

---

## Λίστα (List)

**Markup:**
```
- Μήλο
- Πορτοκάλι
- Μπανάνα
```

**HTML:**
```html
<ul>
<li>Μήλο</li><li>Πορτοκάλι</li><li>Μπανάνα</li>
</ul>
```

---

## Κώδικας (Code Block)

**Markup:**
```
```
Console.WriteLine("Γειά σου SlimeMarkUp!");
```
```

**HTML:**
```html
<pre>
Console.WriteLine("Γειά σου SlimeMarkUp!");
</pre>
```

---

## Έμφαση (Emphasis)

**Markup:**
```
Αυτό είναι **έντονο** και αυτό είναι *πλάγιο*.
```

**HTML:**
```html
Αυτό είναι <strong>έντονο</strong> και αυτό είναι <em>πλάγιο</em>.
```

---

## Συνδυασμός (Combination)

**Markup:**
```
# Λίστα με Κώδικα

- Κώδικας:
```
int x = 5;
```
- Έντονο: **σημαντικό**
```

**HTML:**
```html
<h1>Λίστα με Κώδικα</h1>
<ul>
<li>Κώδικας:
<pre>
int x = 5;
</pre>
</li><li>Έντονο: <strong>σημαντικό</strong></li>
</ul>
```

---

## Γενικός Κανόνας
- Τα headers (`#`) μετατρέπονται σε `<h1>`, `<h2>`, κ.λπ.
- Οι λίστες σε `<ul><li>...</li></ul>`
- Τα blocks κώδικα σε `<pre>...</pre>`
- **Έντονο** με `**` → `<strong>`
- *Πλάγιο* με `*` → `<em>`