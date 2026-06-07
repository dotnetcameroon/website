export interface PagedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface AuthMeResponse {
  email: string;
  role: string;
}

// Events
export type EventStatus = 'Draft' | 'ComingSoon' | 'Passed' | 'Cancelled';
export type EventType = 'Conference' | 'Webinar' | 'Meetup' | 'Workshop';
export type EventHostingModel = 'Online' | 'InPerson' | 'Hybrid';

export interface EventScheduleDto {
  start: string;
  end: string | null;
  isAllDay: boolean;
}

export interface ActivityScheduleDto {
  start: string;
  end: string;
}

export interface HostDto {
  name: string;
  email: string;
  imageUrl: string;
}

export interface EventSummary {
  id: string;
  title: string;
  schedule: EventScheduleDto;
  status: EventStatus;
  type: EventType;
  hostingModel: EventHostingModel;
}

export interface ActivityResponse {
  id: string;
  title: string;
  description: string;
  host: HostDto;
  schedule: ActivityScheduleDto;
}

export interface PartnerResponse {
  id: string;
  name: string;
  logo: string;
  website: string;
}

export interface EventDetail extends EventSummary {
  description: string;
  attendance: number;
  registrationLink: string;
  imageUrl: string;
  images: string;
  location: string | null;
  activities: ActivityResponse[];
  partners: PartnerResponse[];
}

export interface ActivityRequest {
  title: string;
  description: string;
  host: HostDto;
  schedule: ActivityScheduleDto;
}

export interface CreateOrUpdateEventRequest {
  title: string;
  description: string;
  schedule: EventScheduleDto;
  type: EventType;
  status: EventStatus;
  hostingModel: EventHostingModel;
  attendance: number;
  registrationLink: string;
  imageUrl: string;
  images: string;
  location: string | null;
  activities: ActivityRequest[];
  partnerIds: string[];
}

// Partners
export interface CreateOrUpdatePartnerRequest {
  name: string;
  logo: string;
  website: string;
}

// Banners
export type BannerVariant = 'Announcement' | 'Advertisement' | 'Promo' | 'Event' | 'Maintenance';

export interface BannerResponse {
  id: string;
  messageEn: string;
  messageFr: string;
  variant: BannerVariant;
  startDate: string;
  endDate: string;
  link: string | null;
  linkLabelEn: string | null;
  linkLabelFr: string | null;
  dismissible: boolean;
  priority: number;
  isEnabled: boolean;
  createdAt: string;
}

export interface CreateOrUpdateBannerRequest {
  messageEn: string;
  messageFr: string;
  variant: BannerVariant;
  startDate: string;
  endDate: string;
  link: string | null;
  linkLabelEn: string | null;
  linkLabelFr: string | null;
  dismissible: boolean;
  priority: number;
  isEnabled: boolean;
}

// External Apps
export interface AppSummary {
  id: string;
  clientName: string;
}

export interface RegisterAppRequest {
  name: string;
}

export interface RegisterAppResponse {
  id: string;
  clientName: string;
  clientSecret: string;
}
