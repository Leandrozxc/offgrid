# OFFGRID — Outside the Ordinary.

[![.NET](https://img.shields.io/badge/.NET-10.0%20%7C%208.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor Server](https://img.shields.io/badge/Blazor-InteractiveServer-512BD4?style=for-the-badge&logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-3.4_CLI-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)](https://tailwindcss.com/)
[![Currency](https://img.shields.io/badge/Currency-PHP_(₱)-000000?style=for-the-badge)](https://en.wikipedia.org/wiki/Philippine_peso)

An editorial, high-contrast monochrome streetwear e-commerce frontend built using **Blazor Web App (.NET 10 / .NET 8)** and **Tailwind CSS (v3.4 CLI)**. 

OFFGRID rejects disposable micro-trends, presenting heavyweight silhouettes, dropped-shoulder cuts, and industrial graphics. The platform is entirely frontend-driven: no external database or payment gateways, featuring real-time client state management and in-memory catalog discovery.

---

## Key Features

### Catalog & Discovery
- **Reactive Multi-Parameter Filtering**: Simultaneously query items across Category, Garment Size, Colorway, and "New Arrivals Only".
- **Bidirectional URL State**: Deep-linking enabled with full synchronization via query parameters (`?category=`, `?sort=`, `?q=`, `?filter=new`).
- **Instant Keyword Search**: Real-time title and taxonomy substring matching without full page reloads.
- **In-Card Interactive Swatches**: Contained double-circle swatches that hot-swap garment preview images directly on hover without bleeding outside card bounds.
- **Card Image Cycling**: Interactive carousel arrows to cycle through product views and colorway imagery.

### Bag & Commerce Simulation
- **Scoped In-Memory Cart (`CartService`)**: Reactive bag management that survives client navigation during the active user session.
- **Variant Consolidation**: Matches duplicate SKU additions (Product ID + Selected Size + Selected Color) into single lines.
- **Inventory Clamping**: Quantities are physically locked to product stock limits.
- **Dynamic Shipping Threshold**: Standard ₱150 delivery fee automatically converts to **Free Shipping** on orders ₱2,500.00 and above.
- **Live Bump Notification**: Real-time CSS scale bounce (`.bag-bump`) triggered whenever items enter the bag.

### Validation & Checkout
- **Strict DataAnnotations**: Form-level validation on names, delivery addresses, and mobile numbers.
- **Philippine Postal Regex**: 4-digit numeric pattern validation (`^\d{4}$`).
- **Deterministic Receipt**: Generates an itemized confirmation manifest and flushes the active bag session upon submission.

---

## Architecture & State Strategy

```text
[ Browser Client ]
       │
       ▼ (SignalR Circuit / InteractiveServer)
[ App.razor ] (@rendermode="InteractiveServer" on HeadOutlet & Routes)
       │
       ├──► [ ProductCatalog ] (Singleton)
       │       └── Scans local wwwroot/images/products at startup
       │       └── Populates Product, Color, and fallback CSS metadata
       │
       └──► [ CartService ] (Scoped)
               └── Manages active user cart items & stock limits
               └── Publishes Action event: OnChanged -> Header Bump & Bag Count
```

- **Interactive Server Mode**: Configured globally in `App.razor`. Page components do not declare individual `@rendermode` directives, preserving unified SignalR circuits.
- **Asset Fallback Protocol**: Image filenames follow `{product-id}-{color}-{view}.jpg`. When physical photography is unavailable, an industrial geometric SPEC placeholder renders using exact model hex codes without network calls.
- **Accessible Motion**: Global CSS media queries detect `prefers-reduced-motion: reduce` and neutralize transitions and ticker marquee loops while preserving essential feedback.

---

## Directory Structure

```text
├── Components/
│   ├── Layout/
│   │   └── MainLayout.razor       # Global scroll-to-top on navigation
│   ├── Pages/
│   │   ├── Checkout.razor         # Form validation & itemized order receipt
│   │   ├── Home.razor             # Campaign hero, ticker marquee, category directory
│   │   ├── ProductDetails.razor   # Sizing validation, swatches, and add-to-bag
│   │   ├── Shop.razor             # Catalog grid, mobile slide-over drawer, live search
│   │   └── ShoppingBag.razor      # Bag line items, shipping threshold, subtotal
│   └── Shared/
│       ├── ProductCard.razor      # Self-contained zero-overflow product card
│       ├── ProductImage.razor     # Case-insensitive image loader & fallback
│       ├── Reveal.razor           # IntersectionObserver animation wrapper
│       ├── SiteFooter.razor       # Global footer with brand manifest
│       └── SiteHeader.razor       # Sticky navigation, search, and dynamic bag counter
├── Models/
│   ├── CartItem.cs                # SKU line-item model
│   └── Product.cs                 # Product, ProductColor, and Peso currency helpers
├── Properties/
│   └── launchSettings.json        # Local development environment and port settings
├── Services/
│   ├── CartService.cs             # Scoped cart state, stock clamping, shipping rules
│   └── ProductCatalog.cs          # Singleton catalog scanner and data provider
├── Styles/
│   └── app.css                    # Tailwind directives, animations, and custom utilities
├── wwwroot/
│   ├── css/app.css                # Compiled Tailwind output
│   ├── images/products/           # Local JPG photography
│   └── js/offgrid.js              # Native DOM interop (scroll, observer)
├── tailwind.config.js             # Monochrome black/white design token config
├── EventApp.csproj                # Project configuration (.NET 10 / .NET 8)
└── Program.cs                     # Service registration & WebApp pipeline
```

---

## Local Development Setup

### Prerequisites
- **.NET SDK** (v8.0 or v10.0+)
- **Node.js** (v20+ recommended) & npm

### Installation & Run

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Leandrozxc/offgrid.git
   cd offgrid
   ```

2. **Install frontend dependencies:**
   ```bash
   npm install
   ```

3. **Start the Tailwind CSS watcher (Terminal 1):**
   ```bash
   npm run watch
   ```
   *(Alternatively: `npm run watch:css`)*

4. **Launch the Blazor application (Terminal 2):**
   ```bash
   dotnet watch
   ```
   *(Or run directly: `dotnet run`)*

5. **Navigate to the application:**
   Open the HTTP address in your browser:
   ```text
   http://localhost:5000
   ```

---

## Design Tokens

| Token | Hex | Role |
| :--- | :--- | :--- |
| `ink` | `#000000` | Pure Black — Typography, primary buttons, structural lines |
| `cream` | `#FFFFFF` | Pure White — Primary canvas, product cards, field inputs |
| `fog` | `#F5F5F5` | Studio Light Gray — Section pacing, card backdrops, chips |
| `concrete` | `#737373` | Industrial Mid-Gray — Metadata, borders, secondary labels |

---

## Author

- **Leandro** ([@lforleandro](https://github.com/lforleandro) / [@Leandrozxc](https://github.com/Leandrozxc))
- Academic Project — Midterms / Web Systems Development
