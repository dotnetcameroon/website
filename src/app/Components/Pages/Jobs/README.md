# Jobs Frontend - Folder Structure

This folder contains all the frontend pages and components related to the Jobs feature.

## 📁 Structure

```
src/app/Components/Pages/Jobs/
├── Index.razor              # Main jobs listing page (route: /jobs)
├── Details.razor            # Job details page (route: /jobs/{jobId})
├── Submit.razor             # Job submission form (route: /jobs/submit)
└── Components/
    └── JobCard.razor        # Reusable job card component
```

## 📄 Files Description

### **Index.razor**
- **Route**: `/jobs`
- **Purpose**: Displays the list of available job opportunities
- **Features**:
  - Hero section with "Post a Job" CTA
  - Search bar with filters (location, type)
  - Active filters display with clear options
  - Grid/List view toggle
  - Coming Soon message when no jobs available
  - Sample job cards for demonstration
  - Call to action section for employers

### **Details.razor**
- **Route**: `/jobs/{jobId}`
- **Purpose**: Shows detailed information about a specific job
- **Features**:
  - Complete job information (title, company, location, type, salary)
  - Required skills display
  - Responsibilities, requirements, and benefits sections
  - Apply button with email or external URL
  - Application deadline and contact information
  - Social sharing buttons (LinkedIn, Twitter, Copy Link)
  - Company information sidebar
  - Similar job opportunities section
  - 404 page when job not found

### **Submit.razor**
- **Route**: `/jobs/submit`
- **Purpose**: Form for employers to submit new job opportunities
- **Features**:
  - Multi-section form (Company Info, Job Details, Application Info)
  - Comprehensive validation
  - Required fields marked with asterisks
  - Form field guidance (placeholders, helper text)
  - Error messages display
  - Success confirmation page
  - Option to submit another job
  - Terms and conditions acceptance

### **Components/JobCard.razor**
- **Purpose**: Reusable component for displaying job cards
- **Usage**: Can be used in any page that needs to display job information
- **Parameters**:
  - `Title` (required): Job title
  - `Company` (required): Company name
  - `Type`: Job type (default: "Full-time")
  - `Location`: Job location
  - `PostedDate`: When the job was posted
  - `Salary`: Salary range
  - `Skills`: Array of required skills
  - `Description`: Job description
  - `JobUrl`: Link to job details
  - `OnClick`: Event callback for custom click handling
  - `AdditionalClasses`: Custom CSS classes
  - `MaxSkillsToShow`: Maximum skills to display (default: 5)

## 🎨 Design Principles

1. **Consistency**: All pages use the same Tailwind CSS classes and design patterns
2. **Responsive**: Mobile-first design with proper breakpoints
3. **Accessibility**: Proper labels, ARIA attributes, keyboard navigation
4. **User Feedback**: Loading states, error messages, success confirmations
5. **SEO**: Meta tags, Open Graph, Twitter Cards on all pages

## 🔄 Data Flow

Currently, all pages use **mock/demo data** for demonstration purposes:
- `Index.razor`: Uses local `JobExample` class
- `Details.razor`: Uses local `JobDetail` and `JobSummary` classes
- `Submit.razor`: Simulates API call with `Task.Delay`

**When backend is ready**, replace with actual API calls to:
- `GET /api/jobs` - List all jobs
- `GET /api/jobs/{id}` - Get job details
- `POST /api/jobs` - Submit new job

## 🚀 Next Steps

### Backend Integration
1. Create `Job` model in `app.domain`
2. Create `IJobService` in `app.business`
3. Implement API endpoints in `app/Api/Jobs`
4. Connect frontend pages to API
5. Add authentication for job submission
6. Implement admin moderation features

### Future Enhancements
- Job favorites/bookmarks
- Email alerts for new jobs
- Advanced search filters
- Job application tracking
- Company profiles
- Analytics dashboard for employers

## 📝 Notes

- All routes are preserved from the original implementation
- Page structure remains unchanged for compatibility
- No breaking changes to existing functionality
- Files are now logically organized by feature
- Easier to maintain and extend

## 🔗 Related Documentation

See `docs/jobs-page-frontend.md` for complete feature documentation.
