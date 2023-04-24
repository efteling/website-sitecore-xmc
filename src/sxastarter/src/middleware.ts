import type { NextRequest, NextFetchEvent } from 'next/server';
import { NextResponse } from 'next/server';
import middleware from 'lib/middleware';

const PUBLIC_FILE = /\.(.*)$/;

// eslint-disable-next-line
export default async function (req: NextRequest, ev: NextFetchEvent) {
  if (
    req.nextUrl.pathname.startsWith('/_next') ||
    req.nextUrl.pathname.includes('/api/') ||
    PUBLIC_FILE.test(req.nextUrl.pathname)
  ) {
    return;
  }

  if (req.nextUrl.locale === 'default') {
    const locale = req.cookies.get('NEXT_LOCALE')?.value || 'nl';

    return NextResponse.redirect(
      new URL(`/${locale}${req.nextUrl.pathname}${req.nextUrl.search}`, req.url)
    );
  }
  return middleware(req, ev);
}

export const config = {
  /*
   * Match all paths except for:
   * 1. /api routes
   * 2. /_next (Next.js internals)
   * 3. /sitecore/api (Sitecore API routes)
   * 4. /- (Sitecore media)
   * 5. all root files inside /public (e.g. /favicon.ico)
   */
  matcher: ['/', '/((?!api/|_next/|sitecore/api/|-/|[\\w-]+\\.\\w+).*)'],
};
