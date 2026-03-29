import { ChevronLeft, ChevronRight } from 'lucide-react';

interface PaginationProps {
  page: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  onPageChange: (page: number) => void;
}

export function Pagination({ page, totalPages, hasPrevious, hasNext, onPageChange }: PaginationProps) {
  if (totalPages <= 1) return null;

  return (
    <div className="flex items-center justify-between mt-4">
      <span className="text-sm text-gray-500">
        Page {page} of {totalPages}
      </span>
      <div className="flex gap-1">
        <button
          onClick={() => onPageChange(page - 1)}
          disabled={!hasPrevious}
          className="btn btn-outline text-sm !px-3 !py-1.5 disabled:opacity-40 disabled:cursor-not-allowed inline-flex items-center gap-1"
        >
          <ChevronLeft size={14} />
          Previous
        </button>
        <button
          onClick={() => onPageChange(page + 1)}
          disabled={!hasNext}
          className="btn btn-outline text-sm !px-3 !py-1.5 disabled:opacity-40 disabled:cursor-not-allowed inline-flex items-center gap-1"
        >
          Next
          <ChevronRight size={14} />
        </button>
      </div>
    </div>
  );
}
