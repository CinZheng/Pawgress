# Pawgress 🐾

Een uitgebreide web-applicatie voor het beheren en volgen van hondentraining en ontwikkeling.

## 📋 Inhoudsopgave
- [Overzicht](#overzicht)
- [Functionaliteiten](#functionaliteiten)
- [Technische Stack](#technische-stack)
- [Installatie](#installatie)
- [Configuratie](#configuratie)
- [Gebruik](#gebruik)
- [API Documentatie](#api-documentatie)
- [Testing](#testing)
- [Bijdragen](#bijdragen)
- [Licentie](#licentie)

## 🎯 Overzicht

Pawgress is een moderne web-applicatie die hondentrainers en eigenaren helpt bij het volgen en beheren van de ontwikkeling van hun honden. Het platform biedt uitgebreide functionaliteit voor het bijhouden van trainingsvoortgang, gezondheidsgegevens en gedragsontwikkeling.

## ✨ Functionaliteiten

### Kernfunctionaliteiten
- **Hondenprofielen**: Beheer van individuele hondenprofielen met details en foto's
- **Trainingsmodules**: Gestructureerde trainingsplannen en lessen
- **Voortgangsregistratie**: Volg de ontwikkeling via quizzen en mijlpalen
- **Notities Systeem**: Vastleggen van observaties en aandachtspunten
- **Sensor Data Integratie**: Monitoring van gezondheid en activiteit

### Gebruikersfuncties
- **Gebruikersbeheer**: Registratie en authenticatie
- **Rolgebaseerde Toegang**: Verschillende rechten voor trainers en eigenaren
- **Favorieten**: Markeer en volg specifieke hondenprofielen
- **Interactief Dashboard**: Overzicht van belangrijke metrics en updates

## 🛠 Technische Stack

### Backend
- **.NET 8.0**: Modern en performant backend framework
- **Entity Framework Core**: ORM voor database interactie
- **SQL Server**: Relationele database
- **JWT Authentication**: Veilige gebruikersauthenticatie

### Frontend
- **React**: Modern frontend framework
- **TypeScript**: Type-veilige JavaScript
- **Axios**: HTTP client voor API communicatie
- **Material-UI**: UI component bibliotheek

### Testing
- **xUnit**: Unit testing framework
- **Moq**: Mocking framework voor tests
- **Jest & React Testing Library**: Frontend testing

## 🚀 Installatie

### Vereisten
- .NET 8.0 SDK
- Node.js (v18+)
- SQL Server
- Git

### Backend Setup
```bash
# Clone de repository
git clone https://github.com/[username]/Pawgress.git

# Navigeer naar de backend directory
cd Pawgress

# Herstel NuGet packages
dotnet restore

# Start de applicatie
dotnet run
```

### Frontend Setup
```bash
# Navigeer naar de frontend directory
cd frontend

# Installeer dependencies
npm install

# Start de development server
npm start
```

## ⚙️ Configuratie

### Database Configuratie
1. Update de connection string in `appsettings.json`
2. Voer database migraties uit:
```bash
dotnet ef database update
```

### Environment Variables
Maak een `.env` bestand aan in de frontend directory:
```
REACT_APP_API_URL=http://localhost:5000/api
```

## 📱 Gebruik

### Eerste Stappen
1. Registreer een nieuw account
2. Log in met je credentials
3. Maak een hondenprofiel aan
4. Begin met het volgen van trainingsmodules

### Trainingsmodules
- Kies een beschikbare module
- Volg de lessen en instructies
- Maak quizzen om voortgang te testen
- Bekijk resultaten en statistieken

## 📚 API Documentatie

De API documentatie is beschikbaar via Swagger UI:
```
http://localhost:5000/swagger
```

### Belangrijke Endpoints
- `/api/DogProfile`: Hondenprofiel management
- `/api/Training`: Trainingsmodule endpoints
- `/api/Quiz`: Quiz en voortgang
- `/api/Notes`: Notitie systeem
- `/api/Users`: Gebruikersbeheer

## 🧪 Testing

### Backend Tests
```bash
# Run alle tests
dotnet test

# Run specifieke test categorie
dotnet test --filter "Category=Integration"
```

### Frontend Tests
```bash
# Run alle tests
npm test

# Run tests met coverage
npm test -- --coverage
```

## 🤝 Bijdragen

We verwelkomen bijdragen aan het Pawgress project! Volg deze stappen:

1. Fork de repository
2. Maak een feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit je wijzigingen (`git commit -m 'Add some AmazingFeature'`)
4. Push naar de branch (`git push origin feature/AmazingFeature`)
5. Open een Pull Request

### Code Standaarden
- Volg de bestaande code stijl
- Schrijf unit tests voor nieuwe functionaliteit
- Update documentatie waar nodig
- Zorg voor duidelijke commit messages

## 📄 Licentie

Dit project is gelicentieerd onder de MIT License - zie het [LICENSE](LICENSE) bestand voor details.

## 🙏 Dankwoord

Speciale dank aan alle bijdragers die hebben geholpen bij het ontwikkelen van Pawgress.

## 📞 Contact

Voor vragen of ondersteuning:
- Email: [support@pawgress.com](mailto:support@pawgress.com)
- GitHub Issues: [https://github.com/[username]/Pawgress/issues](https://github.com/[username]/Pawgress/issues)

---

Gemaakt met ❤️ voor honden en hun trainers
