import { useState, useRef, useEffect } from "react";

type DropdownItem = {
  label: string;
  value: string;
  imageUrl: string;
};

interface Props {
  items: DropdownItem[];
  selected: string;
  onChange: (value: string) => void;
  className?: string; // e.g. "w-48" for Tailwind or "200px" as inline style
}

const ImageLabelDropdown: React.FC<Props> = ({
  items,
  selected,
  onChange,
  className = "w-48",
}) => {
  const [open, setOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const selectedItem = items.find((item) => item.value === selected);

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(event.target as Node)
      ) {
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <div className={`relative ${className}`} ref={dropdownRef}>
      <button
        onClick={() => setOpen((prev) => !prev)}
        className="w-full flex items-center justify-between px-3 py-2 bg-white border border-gray-300 rounded shadow-sm hover:bg-gray-200"
      >
        <div className="flex items-center space-x-2">
          <img
            src={selectedItem?.imageUrl}
            alt={selectedItem?.label}
            className="w-5 h-5 rounded-full object-cover"
          />
          <span className="text-sm text-slate-500">{selectedItem?.label}</span>
        </div>
        <svg
          className="w-4 h-4 text-gray-500 ml-2"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path d="M19 9l-7 7-7-7" strokeWidth={2} strokeLinecap="round" />
        </svg>
      </button>

      {open && (
        <ul className="absolute left-8 min-w-full z-10 mt-2 bg-white border border-gray-200 rounded shadow-lg">
          {items.map((item) => (
            <li key={item.value} className="min-w-full">
              <button
                onClick={() => {
                  onChange(item.value);
                  setOpen(false);
                }}
                className="min-w-full flex items-center space-x-2 px-3 py-2 hover:bg-gray-200 text-left"
              >
                <img
                  src={item.imageUrl}
                  alt={item.label}
                  className="w-5 h-5 rounded-full object-cover"
                />
                <span className="text-sm text-slate-500">{item.label}</span>
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default ImageLabelDropdown;
