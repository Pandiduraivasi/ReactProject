import { Link, LinkProps } from "react-router-dom";
import { forwardRef } from "react";

interface CustomLinkProps extends LinkProps {
  children: React.ReactNode;
}

const CustomLink = forwardRef<HTMLAnchorElement, CustomLinkProps>(
  ({ to, children, ...props }, ref) => {
    return (
      <Link to={to} {...props} ref={ref} className="text-blue-500 hover:underline">
        {children}
      </Link>
    );
  }
);

export default CustomLink;
