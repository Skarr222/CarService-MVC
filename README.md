# CarService-MVC
A web-based car service management system built with ASP.NET Core MVC (.NET 8). The solution consists of two applications: an Intranet for employees (managing repair orders, clients, vehicles, services, and CMS content) and a customer-facing Portal (browsing services, booking appointments, tracking repair status). College project.
# 🔧 AutoSerwis Pro — Blueprint projektu

## Opis

System do zarządzania serwisem samochodowym. Dwie aplikacje MVC w C#:
- **Intranet** — panel dla pracowników (mechanicy, recepcja, admin)
- **Portal** — strona dla klientów (umawianie wizyt, sprawdzanie statusu, historia)

---

## Struktura Solution

```
AutoSerwis.sln
├── AutoSerwis.Data            // modele, DbContext, migracje, seedy
├── AutoSerwis.Intranet        // ASP.NET MVC — panel pracownika
└── AutoSerwis.Portal          // ASP.NET MVC — strona klienta
```

Wspólna baza danych (Entity Framework Core), oba projekty referencjonują `AutoSerwis.Data`.

---

## Klasy modelu (10 klas)

### 1. Client (Klient)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| FirstName | string | Imię |
| LastName | string | Nazwisko |
| Email | string | E-mail |
| Phone | string | Telefon |
| CreatedAt | DateTime | Data rejestracji |

**Relacje:** Client 1 → N Vehicle, Client 1 → N RepairOrder

---

### 2. Vehicle (Pojazd)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| ClientId | int | FK → Client |
| Brand | string | Marka (np. Toyota) |
| Model | string | Model (np. Corolla) |
| Year | int | Rok produkcji |
| LicensePlate | string | Numer rejestracyjny |
| VIN | string | Numer VIN |
| EngineType | string | Typ silnika (benzyna/diesel/elektryczny/hybryda) |

**Relacje:** Vehicle N → 1 Client, Vehicle 1 → N RepairOrder

---

### 3. Employee (Pracownik)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| FirstName | string | Imię |
| LastName | string | Nazwisko |
| Position | string | Stanowisko (Mechanik / Recepcja / Kierownik) |
| Phone | string | Telefon |
| Email | string | E-mail |
| HireDate | DateTime | Data zatrudnienia |
| IsActive | bool | Czy aktywny |

**Relacje:** Employee 1 → N RepairOrder (jako przypisany mechanik)

---

### 4. ServiceCategory (Kategoria usług)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| Name | string | Nazwa kategorii (np. "Wymiana opon", "Diagnostyka") |
| Description | string | Opis |
| IconCss | string | Klasa CSS ikony (do wyświetlania na Portalu) |

**Relacje:** ServiceCategory 1 → N Service

---

### 5. Service (Usługa)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| ServiceCategoryId | int | FK → ServiceCategory |
| Name | string | Nazwa usługi (np. "Wymiana klocków hamulcowych") |
| Description | string | Szczegółowy opis |
| EstimatedPrice | decimal | Szacunkowa cena |
| EstimatedDuration | int | Szacowany czas w minutach |

**Relacje:** Service N → 1 ServiceCategory, Service N → N RepairOrder (przez RepairOrderService)

---

### 6. RepairOrder (Zlecenie naprawy)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| ClientId | int | FK → Client |
| VehicleId | int | FK → Vehicle |
| EmployeeId | int | FK → Employee (przypisany mechanik) |
| Description | string | Opis usterki / zlecenia |
| Status | enum | Nowe / Przyjęte / WTrakcie / Gotowe / Wydane / Anulowane |
| CreatedAt | DateTime | Data utworzenia |
| PlannedDate | DateTime? | Planowany termin realizacji |
| CompletedAt | DateTime? | Data zakończenia |
| TotalCost | decimal? | Koszt końcowy |
| Notes | string? | Notatki mechanika |

**Relacje:** RepairOrder N → 1 Client, N → 1 Vehicle, N → 1 Employee, 1 → N RepairOrderService

---

### 7. RepairOrderService (tabela łącząca Zlecenie ↔ Usługa)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| RepairOrderId | int | FK → RepairOrder |
| ServiceId | int | FK → Service |
| Quantity | int | Ilość |
| UnitPrice | decimal | Cena jednostkowa (w momencie zlecenia) |
| Notes | string? | Uwagi do konkretnej usługi |

**Relacje:** N → 1 RepairOrder, N → 1 Service

---

### 8. CmsContent (Treści CMS)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| Key | string | Unikalny klucz (np. "home_hero_title", "about_text", "contact_address") |
| Title | string? | Tytuł sekcji |
| Content | string | Treść HTML/tekst |
| Section | string | Sekcja portalu (Home / About / Contact / Services / Footer) |
| SortOrder | int | Kolejność wyświetlania |
| IsActive | bool | Czy widoczne na portalu |
| UpdatedAt | DateTime | Ostatnia edycja |

**Cel:** Wszystkie teksty na Portalu (nagłówki, opisy, adresy, godziny otwarcia) zarządzane z Intranetu → spełnia wymóg "tekstami steruj z poziomu Intranetu".

---

### 9. Testimonial (Opinia klienta)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| ClientName | string | Imię klienta (lub "Anonim") |
| Content | string | Treść opinii |
| Rating | int | Ocena 1-5 |
| IsApproved | bool | Zatwierdzona do wyświetlenia |
| CreatedAt | DateTime | Data dodania |

**Relacje:** brak FK (opinię może dodać klient portalu, a admin zatwierdza w Intranecie)

---

### 10. ContactRequest (Wiadomość z formularza kontaktowego)

| Pole | Typ | Opis |
|------|-----|------|
| Id | int | PK |
| Name | string | Imię |
| Email | string | E-mail |
| Phone | string? | Telefon |
| Subject | string | Temat |
| Message | string | Treść |
| IsRead | bool | Czy przeczytana |
| CreatedAt | DateTime | Data wysłania |
| ResponseNote | string? | Notatka z odpowiedzi (wewnętrzna) |

---

## Diagram relacji (ERD)

```
Client ──────── 1:N ──────── Vehicle
  │                            │
  │ 1:N                        │ 1:N
  ▼                            ▼
RepairOrder ──── N:1 ──── Employee
  │
  │ 1:N
  ▼
RepairOrderService ──── N:1 ──── Service ──── N:1 ──── ServiceCategory

CmsContent        (bez relacji — standalone CMS)
Testimonial       (bez FK — standalone)
ContactRequest    (bez FK — standalone)
```

---

## Intranet — funkcjonalności

### CRUD na wszystkich 10 klasach (wymóg: min. 8):

| Klasa | Create | Read/List | Update | Delete | Uwagi |
|-------|--------|-----------|--------|--------|-------|
| Client | ✅ | ✅ (lista + szczegóły) | ✅ | ✅ (soft delete) | Widok szczegółów z pojazdami i historią zleceń |
| Vehicle | ✅ | ✅ | ✅ | ✅ | Dropdown z klientami |
| Employee | ✅ | ✅ | ✅ | ✅ (dezaktywacja) | Filtrowanie po stanowisku |
| ServiceCategory | ✅ | ✅ | ✅ | ✅ | |
| Service | ✅ | ✅ | ✅ | ✅ | Dropdown z kategoriami |
| RepairOrder | ✅ | ✅ | ✅ | ✅ | Zmiana statusu, przypisanie mechanika, dodawanie usług |
| RepairOrderService | ✅ | ✅ | ✅ | ✅ | Zarządzane w kontekście zlecenia |
| CmsContent | ✅ | ✅ | ✅ | ✅ | Edytor tekstów portalu |
| Testimonial | ✅ | ✅ | ✅ (zatwierdzanie) | ✅ | Moderacja opinii |
| ContactRequest | — | ✅ | ✅ (oznacz jako przeczytane) | ✅ | Tylko odczyt + status |

### Widoki Intranetu:
- **Dashboard** — liczba aktywnych zleceń, dziś zaplanowane, nowe wiadomości kontaktowe, najnowsze zlecenia
- **Lista zleceń** — filtrowanie po statusie, kliencie, mechaniku, dacie; sortowanie
- **Szczegóły zlecenia** — podgląd klienta, pojazdu, lista usług, zmiana statusu, notatki
- **Zarządzanie CMS** — lista kluczy, edycja treści, podgląd sekcji portalu

---

## Portal — funkcjonalności (wymóg: min. 6 klas)

Klasy używane na Portalu: **Client, Vehicle, RepairOrder, Service, ServiceCategory, CmsContent, Testimonial, ContactRequest**

### Strony Portalu:

| Strona | Opis | Klasy |
|--------|------|-------|
| **Strona główna** | Hero section, skrót usług, opinie, CTA | CmsContent, Testimonial, ServiceCategory |
| **Usługi** | Kategorie → lista usług z cenami i czasem | ServiceCategory, Service |
| **Umów wizytę** | Formularz: dane klienta + pojazd + wybór usług + termin | Client, Vehicle, RepairOrder, Service |
| **Sprawdź status** | Klient wpisuje e-mail + nr rejestracyjny → widzi status zlecenia | RepairOrder, Vehicle |
| **Opinie** | Lista zatwierdzonych opinii + formularz dodawania | Testimonial |
| **Kontakt** | Formularz kontaktowy, mapa, godziny otwarcia, adres | ContactRequest, CmsContent |
| **O nas** | Opis firmy, zespół | CmsContent, Employee |

### Sterowanie tekstami z Intranetu (wymóg 6 pkt):

Każdy tekst na Portalu pochodzi z tabeli `CmsContent`. Przykładowe klucze:

```
home_hero_title        → "Profesjonalny serwis samochodowy"
home_hero_subtitle     → "Zaufaj ekspertom z 20-letnim doświadczeniem"
home_about_text        → "Nasza firma powstała w..."
services_intro         → "Oferujemy szeroki zakres usług..."
about_description      → "AutoSerwis Pro to rodzinna firma..."
contact_address        → "ul. Mechaników 15, 00-001 Warszawa"
contact_hours          → "Pon-Pt: 8:00-18:00, Sob: 9:00-14:00"
contact_phone          → "+48 123 456 789"
footer_copyright       → "© 2026 AutoSerwis Pro"
footer_slogan          → "Twój samochód w dobrych rękach"
```

W kontrolerze Portalu:
```csharp
// Przykład w HomeController
public async Task<IActionResult> Index()
{
    var contents = await _context.CmsContents
        .Where(c => c.Section == "Home" && c.IsActive)
        .OrderBy(c => c.SortOrder)
        .ToDictionaryAsync(c => c.Key, c => c);

    return View(contents);
}
```

W widoku Razor:
```html
<h1>@Model["home_hero_title"].Content</h1>
<p>@Model["home_hero_subtitle"].Content</p>
```

---

## Dodatkowe elementy (na 20 punktów)

Wybierz 4-5 z poniższych, każdy wart ~4-5 pkt:

### 1. Autoryzacja i role (ASP.NET Identity)
- Role: Admin, Mechanik, Recepcja
- Admin widzi wszystko, Mechanik tylko swoje zlecenia, Recepcja zarządza klientami i zleceniami
- Logowanie / rejestracja / zarządzanie kontami

### 2. Wyszukiwanie i filtrowanie AJAX
- Dynamiczne filtrowanie tabel bez przeładowania strony
- Wyszukiwarka klientów/zleceń po frazie
- Biblioteka: jQuery + AJAX lub fetch API

### 3. Statusy zleceń z historią zmian
- Nowa tabela `RepairOrderStatusHistory` (Id, RepairOrderId, OldStatus, NewStatus, ChangedBy, ChangedAt, Comment)
- Timeline zmian statusu widoczny w szczegółach zlecenia
- Klient na Portalu widzi postęp naprawy

### 4. Generowanie PDF (faktura / potwierdzenie)
- Po zakończeniu zlecenia — generowanie PDF z podsumowaniem
- Biblioteka: QuestPDF lub iTextSharp
- Potwierdzenie umówienia wizyty do pobrania

### 5. Dashboard z wykresami (Chart.js)
- Liczba zleceń miesięcznie
- Przychody w czasie
- Najpopularniejsze usługi
- Obłożenie mechaników

### 6. Walidacja FluentValidation
- Zamiast DataAnnotations — FluentValidation
- Walidacja po stronie serwera + komunikaty po polsku
- Np. VIN musi mieć 17 znaków, rejestracja w formacie polskim

### 7. Upload zdjęć
- Mechanik dodaje zdjęcia usterki / naprawy do zlecenia
- Nowa tabela `RepairPhoto` (Id, RepairOrderId, FilePath, Description, UploadedAt)
- Galeria w szczegółach zlecenia

### 8. Powiadomienia e-mail
- Po zmianie statusu zlecenia → mail do klienta
- Po wypełnieniu formularza kontaktowego → mail do serwisu
- Biblioteka: MailKit

### 9. AutoComplete / Select2
- Wyszukiwanie klientów po imieniu/nazwisku z autouzupełnianiem
- Wybór pojazdu zależny od wybranego klienta (kaskadowe dropdown)
- Biblioteka: Select2 lub TomSelect

### 10. Eksport danych do Excel
- Lista zleceń / klientów / przychody do pliku .xlsx
- Biblioteka: ClosedXML lub EPPlus

---

## Enum StatusZlecenia

```csharp
public enum RepairStatus
{
    Nowe = 0,
    Przyjete = 1,
    WTrakcie = 2,
    OczekiwanieNaCzesci = 3,
    Gotowe = 4,
    Wydane = 5,
    Anulowane = 6
}
```

---

## Seed Data (przykładowe dane)

Przygotuj seedy, żeby od razu było co pokazać:

- 5-10 klientów z pojazdami
- 3-4 pracowników (mechanicy + recepcja)
- 4-5 kategorii usług, po 3-5 usług w każdej
- 10-15 zleceń w różnych statusach
- 10+ wpisów CmsContent dla wszystkich sekcji portalu
- 5 opinii (3 zatwierdzone, 2 do moderacji)
- 3 wiadomości kontaktowe

---

## Przykładowe kategorie usług i usługi

| Kategoria | Usługi |
|-----------|--------|
| Mechanika | Wymiana klocków hamulcowych, Wymiana tarcz, Wymiana oleju, Wymiana rozrządu |
| Opony | Wymiana opon, Wyważanie kół, Przechowywanie opon |
| Diagnostyka | Diagnostyka komputerowa, Przegląd techniczny, Kontrola geometrii |
| Elektryka | Wymiana akumulatora, Naprawa instalacji, Wymiana alternator |
| Klimatyzacja | Serwis klimatyzacji, Odgrzybianie, Uzupełnienie czynnika |

---

## Technologie

| Warstwa | Technologia |
|---------|------------|
| Backend | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| Baza danych | SQL Server (LocalDB) |
| Frontend Intranet | Bootstrap 5 + DataTables.js |
| Frontend Portal | Bootstrap 5 + custom CSS |
| Autoryzacja | ASP.NET Identity |
| Walidacja | FluentValidation lub DataAnnotations |
| Wykresy | Chart.js (dashboard Intranetu) |
| PDF | QuestPDF |

---

## Kolejność implementacji (sugerowana)

1. `AutoSerwis.Data` — modele, DbContext, migracje, seedy
2. Intranet — CRUD dla prostych klas (Employee, ServiceCategory, Service)
3. Intranet — CRUD klienci + pojazdy + zlecenia (bardziej złożone)
4. Intranet — CmsContent (zarządzanie treściami)
5. Portal — layout + strona główna z danymi z CMS
6. Portal — strona usług, "o nas", kontakt
7. Portal — formularz "umów wizytę" + "sprawdź status"
8. Dodatkowe elementy (Identity, wykresy, PDF, AJAX itp.)
9. Styling i polerowanie wyglądu