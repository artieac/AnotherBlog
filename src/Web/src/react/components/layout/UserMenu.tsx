import React, { useState, useEffect } from 'react';
import { IUser } from '@/types/user.types';

interface UserMenuProps {
    user: IUser;
}

export const UserMenu: React.FC<UserMenuProps> = ({ user }) => {
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    useEffect(() => {
        const closeMenu = () => setIsMenuOpen(false);
        if (isMenuOpen) {
            window.addEventListener('click', closeMenu);
        }
        return () => window.removeEventListener('click', closeMenu);
    }, [isMenuOpen]);

    return (
        <div className="relative">
            <button
                onClick={(e) => {
                    e.stopPropagation();
                    setIsMenuOpen(!isMenuOpen);
                }}
                className="flex items-center gap-2 px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700 focus:outline-none transition-colors"
            >
                <span>{user.DisplayName}</span>
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className={`transition-transform ${isMenuOpen ? 'rotate-180' : ''}`}><path d="m6 9 6 6 6-6"/></svg>
            </button>
            {isMenuOpen && (
                <div className="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 ring-1 ring-black ring-opacity-5 z-50 overflow-hidden">
                    <a href="/User/Preferences" className="flex items-center gap-2 px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 transition-colors">
                        <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="opacity-60"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
                        Account
                    </a>
                    <form action="/User/Logout" method="post">
                        <button type="submit" className="flex w-full items-center gap-2 px-4 py-2 text-sm text-red-600 hover:bg-red-50 transition-colors border-t border-gray-100">
                            <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="opacity-60"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><line x1="21" y1="12" x2="9" y2="12"/></svg>
                            Sign Out
                        </button>
                    </form>
                </div>
            )}
        </div>
    );
};
